using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoundState
{ 
    Distribution,   //카드분배
    PlayerTurn,     //플레이어 턴 
    OpponentTurn,   //상대 턴
    Resolve,        //판정
    GoStop,         //고스톱 선택
    Settlement,     //정산
    End             //라운드 끝
}


public class RoundManager : Singleton<RoundManager>
{
    protected override bool IsDontDestroyOnLoad => false;

    public static event Action<RoundState> OnRoundStateChanged;

    private List<CardData> tableCards = new List<CardData>();

    private ScoreManager humanScore;
    private ScoreManager aiScore;

    private GoStopManager humanGoStop;
    private GoStopManager aiGoStop;

    private HumanPlayer humanPlayer;
    private AIPlayer aiPlayer;

    private TurnResolver turnResolver;

    private Coroutine roundRoutine;
    private Coroutine arrangeRoutine;

    //Stop선언 종료 플래그
    private bool isStoped = false;

    public RoundState CurrentState;

    protected override void Awake()
    {
        base.Awake();

        humanPlayer = new HumanPlayer("Human");
        aiPlayer = new AIPlayer("AI");

        turnResolver = new TurnResolver();

        humanScore = new ScoreManager();
        aiScore = new ScoreManager();

        humanGoStop = new GoStopManager();
        aiGoStop = new GoStopManager();

    }

    public void StartRound()
    {
        isStoped = false;

        if(roundRoutine != null)
        {
            StopCoroutine(roundRoutine);
        }
        roundRoutine = StartCoroutine(RoundRoutine());
    }

    private IEnumerator RoundRoutine()
    {
        //====================Distribution=====================
        ChangeState(RoundState.Distribution);

        DeckManager.Instance.InitializeDeck(true);
        tableCards.Clear();

        humanPlayer.Hand.Clear();
        aiPlayer.Hand.Clear();

        for(int i = 0; i < 8; i++)
        {
            tableCards.Add(DeckManager.Instance.Draw());
        }
        for(int i = 0; i < 10; i++)
        {
            humanPlayer.Hand.Add(DeckManager.Instance.Draw());
            aiPlayer.Hand.Add(DeckManager.Instance.Draw());
        }

        Debug.Log("===== 카드 분배 완료 =====");
        Debug.Log($"Human Hand : {humanPlayer.Hand.Count}장");
        Debug.Log($"AI Hand    : {aiPlayer.Hand.Count}장");
        Debug.Log($"Table      : {tableCards.Count}장");
        Debug.Log($"Deck Remain: {DeckManager.Instance.Count}장");
        
        ShowDistributionViews();
        RequestArrangeHands();
        //======================================================
        //======================Turn Loop=======================
        while (true)
        {
            //============PlayerTurn============
            ChangeState(RoundState.PlayerTurn);
            humanScore.BeginTurn();
            
            while(true)
            {
                if(humanPlayer.SelectIndex >=0)
                {
                    turnResolver.ExecuteTurn(humanPlayer, tableCards);
                    break;
                }
                yield return null;
            }

            RefreshHandAndTableView();

            //점수 업뎃 로그 -> 추후 삭제
            int before = humanScore.CurrentScore;
            humanScore.EndTurnAndRecalculate(humanPlayer);
            Debug.Log($"[Score] Human : {before} → {humanScore.CurrentScore}");
            
            humanGoStop.JudgeAfterTurn(humanScore);
            
            yield return StartCoroutine(HandleGoStop(humanPlayer, humanScore, humanGoStop));
            if(CheckRoundEnd())
            {
                break;
            }
            //==============AITurn==============
            ChangeState(RoundState.OpponentTurn);
            aiScore.BeginTurn();
            turnResolver.ExecuteTurn(aiPlayer, tableCards);

            RefreshHandAndTableView();

            //점수 업뎃 로그 -> 추후 삭제
            int aiBefore = aiScore.CurrentScore;
            aiScore.EndTurnAndRecalculate(aiPlayer);
            Debug.Log($"[Score] AI : {aiBefore} → {aiScore.CurrentScore}");
            
            aiGoStop.JudgeAfterTurn(aiScore);

            yield return StartCoroutine(HandleGoStop(aiPlayer, aiScore, aiGoStop));
            if(CheckRoundEnd())
            {
                break;
            }

            yield return null;   
        }
        //======================================================
        //=========================End==========================
        ChangeState(RoundState.End);

        humanGoStop.Reset();
        aiGoStop.Reset();

        Debug.Log("========== Round End ==========");
    }

    private IEnumerator HandleGoStop(Player player, ScoreManager score, GoStopManager goStop)
    {
        ChangeState(RoundState.GoStop);

        if(!goStop.CanGo(score) && !goStop.CanStop(score))
        {
            yield break;
        }

        //GoStop선택 로그 -> 추후 삭제
        Debug.Log($"[GoStop] {player.Name} 현재 점수 = {score.CurrentScore}");
        Debug.Log("[GoStop] 선택 대기중... (G = Go / S = Stop)");
        
        bool decided = false;

        while(!decided)
        {
            if(player is HumanPlayer)
            {
                if(Input.GetKeyDown(KeyCode.G) && goStop.CanGo(score))
                {
                    Debug.Log("[Input] Human pressed G");
                    goStop.LetsGo(score);
                    Debug.Log("[Game] Go 선택 → 게임 계속");
                    decided = true;
                }
                else if(Input.GetKeyDown(KeyCode.S) && goStop.LetsStop(score))
                {
                    Debug.Log("[Input] Human pressed S");
                    Debug.Log("[Game] Stop 선택 → 라운드 종료");
                    isStoped = true;
                    decided = true;
                }
            }
            else
            {
                if(goStop.CanStop(score))
                {
                    Debug.Log("[AI] Stop 선택");
                    isStoped = true;
                }
                else
                {
                    Debug.Log("[AI] Go 선택");
                    goStop.LetsGo(score);
                }
                decided = true;
            }

            yield return null;
        }
    }

    private bool CheckRoundEnd()
    {
        if(DeckManager.Instance.IsEmpty())
        {
            Debug.Log("덱 소진. 라운드 종료");
            return true;
        }
        if(isStoped)
        {
            Debug.Log("Stop 선언. 라운드 종료");
            return true;
        }

        return false;
    }

    private void ChangeState(RoundState next)
    {
        CurrentState = next;
        Debug.Log($"[RoundManager] State -> {next}");
    }

    // 숫자 누르면 Human만 선택 인덱스
    public void SetHumanSelectIndex(int index)
    {
        humanPlayer.SetSelectIndex(index);
        Debug.Log($"[Human Select] index={index}");
    }

    //카드 시각화 메서드
    private void ShowDistributionViews()
    {
        CardViewManager.Instance.ClearArea(CardAreaType.HumanHandCard);
        CardViewManager.Instance.ClearArea(CardAreaType.AIHandCard);
        CardViewManager.Instance.ClearArea(CardAreaType.TableCard);

        foreach(var card in tableCards)
        {
           var v = CardViewManager.Instance.GetCard(card, CardAreaType.TableCard, true);
            v.BindHandIndex(-1, clickable: false);
        }

        for(int i = 0; i<humanPlayer.Hand.Count; i++)
        {
            var v = CardViewManager.Instance.GetCard(humanPlayer.Hand[i], CardAreaType.HumanHandCard, true);
            v.BindHandIndex(i, clickable: true);
        }

        for (int i = 0; i < aiPlayer.Hand.Count; i++)
        {
            var v = CardViewManager.Instance.GetCard(aiPlayer.Hand[i], CardAreaType.AIHandCard, false);
            v.BindHandIndex(-1, clickable: false);
        }
    }

    //손패/테이블 갱신 메서드
    private void RefreshHandAndTableView()
    {
        //HumanHand
        CardViewManager.Instance.ClearArea(CardAreaType.HumanHandCard);

        for (int i = 0; i < humanPlayer.Hand.Count; i++)
        {
            var view = CardViewManager.Instance.GetCard(
                humanPlayer.Hand[i],
                CardAreaType.HumanHandCard,
                true
            );
            view.BindHandIndex(i, true);
        }

        //AIHand
        CardViewManager.Instance.ClearArea(CardAreaType.AIHandCard);
        List<Transform> aiHandCards = new List<Transform>();

        for (int i = 0; i < aiPlayer.Hand.Count; i++)
        {
            var view = CardViewManager.Instance.GetCard(
                aiPlayer.Hand[i],
                CardAreaType.AIHandCard,
                false
            );

            view.BindHandIndex(-1, clickable: false);
        }

        //Table
        CardViewManager.Instance.ClearArea(CardAreaType.TableCard);
        foreach(var card in tableCards)
        {
            var v = CardViewManager.Instance.GetCard(card, CardAreaType.TableCard, true);
            v.BindHandIndex(-1, clickable: false);
        }

        RequestArrangeHands();
    }

    private void ArrangeHands()
    {
        ArrangeHandArea(CardAreaType.HumanHandCard);
        ArrangeHandArea(CardAreaType.AIHandCard);
    }

    private void ArrangeHandArea(CardAreaType areaType)
    {
        var area = CardViewManager.Instance.GetAreaTransform(areaType);
        if (area == null) return;

        var layout = area.GetComponent<HandFanLayOut>();
        if (layout == null)
        {
            Debug.LogWarning($"[{nameof(RoundManager)}] {areaType}에 HandFanLayout이 없습니다.");
            return;
        }

        List<Transform> cards = new List<Transform>(area.childCount);
        for (int i = 0; i < area.childCount; i++)
        {
            var child = area.GetChild(i);
            if(!child.gameObject.activeSelf)
            {
                continue;
            }
            cards.Add(child);
        }
            

        layout.Arrange(cards);
    }

    private void RequestArrangeHands()
    {
        if (arrangeRoutine != null)
            StopCoroutine(arrangeRoutine);

        arrangeRoutine = StartCoroutine(CoArrangeHandsNextFrame());
    }

    private IEnumerator CoArrangeHandsNextFrame()
    {
        // 1프레임 대기: UI 레이아웃 / Destroy 정리 / Instantiate 반영
        yield return null;

        // (옵션) 강제 레이아웃 갱신
        Canvas.ForceUpdateCanvases();

        ArrangeHands();
        arrangeRoutine = null;
    }
}
