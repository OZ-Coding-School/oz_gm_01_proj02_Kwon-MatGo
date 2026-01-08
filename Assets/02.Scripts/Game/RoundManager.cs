using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoundState
{ 
    Init,           //라운드 초기화
    Distribution,   //카드분배
    PlayerTrun,     //플레이어 턴 
    OpponentTrun,   //상대 턴
    Resolve,        //판정
    GoStop,         //고스톱 선택
    Settlement,     //정산
    End             //라운드 끝
}


public class RoundManager : Singleton<RoundManager>
{
    protected override bool IsDontDestroyOnLoad => false;

    public static event Action<RoundState> OnRoundStateChanged;
    public RoundState CurrentState { get; private set; }

    private List<CardData> tableCards = new List<CardData>();

    private HumanPlayer humanPlayer;
    private AIPlayer aiPlayer;


    private TurnResolver turnResolver;

    private Coroutine roundRoutine;

    protected override void Awake()
    {
        base.Awake();

        humanPlayer = new HumanPlayer("Human");
        aiPlayer = new AIPlayer("AI");
        turnResolver = new TurnResolver();

    }

    public void StartRound()
    {
        if(roundRoutine != null)
        {
            StopCoroutine(roundRoutine);
        }
        roundRoutine = StartCoroutine(RoundRoutine());
    }

    private IEnumerator RoundRoutine()
    {
        ChangeState(RoundState.Init);
        yield return null;

        ChangeState(RoundState.Distribution);
        DistributeCards_Log();
        yield return null;

        while (true)
        {
            ChangeState(RoundState.PlayerTrun);
            turnResolver.ExecuteTurn(humanPlayer, tableCards);
            yield return null;

            ChangeState(RoundState.OpponentTrun);
            turnResolver.ExecuteTurn(aiPlayer, tableCards);
            yield return null;

            //종료조건 만들어야됨
        }
    }


    private void DistributeCards_Log()
    {
        Debug.Log("===== 초기 준비 시작 =====");

        //덱 초기화
        DeckManager.Instance.InitializeDeck(shuffle: true);

        tableCards.Clear();
        humanPlayer.Hand.Clear();
        aiPlayer.Hand .Clear();
        humanPlayer.CapturedCards .Clear();
        aiPlayer .CapturedCards .Clear();

        Debug.Log("바닥 카드 8장 세팅");
        for(int i = 0; i<8; i++)
        {
            var card = DeckManager.Instance.Draw();
            tableCards.Add(card);
            Debug.Log($"Table[{i}] : {card.DebugName}");
        }

        Debug.Log("플레이어 손 10장");
        for (int i = 0; i < 10; i++)
        {
            var player = DeckManager.Instance.Draw();
            humanPlayer.Hand.Add(player);
            Debug.Log($"Player[{i}] : {player.DebugName}");
        }

        Debug.Log("AI 손 10장");
        for (int i = 0; i < 10; i++)
        {
            var ai = DeckManager.Instance.Draw();
            aiPlayer.Hand.Add(ai);
            Debug.Log($"AI[{i}] : {ai.DebugName}");
        }

        Debug.Log("===== 초기 준비 완료 =====");
    }
    private void ChangeState(RoundState next)
    {
        CurrentState = next;
        Debug.Log($"[RoundManager] State -> {next}");
        OnRoundStateChanged?.Invoke(next);
    }

    // 숫자 누르면 Human만 선택 인덱스
    public void SetHumanSelectIndex(int index)
    {
        humanPlayer.SetSelectIndex(index);
        Debug.Log($"[Human Select] index={index}");
    }

}
