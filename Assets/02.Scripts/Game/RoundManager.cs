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
    public static event Action<Player> OnTurnStarted;
    public static event Action<Player> OnTurnEnded;

    public RoundState CurrentState { get; private set; }

    private List<CardData> tableCards = new List<CardData>();

    private Player humanPlayer;
    private Player aiPlayer;
    private Player currentTurnPlayer;

    private TurnResolver turnResolver;
    private CaptureResolver captureResolver;

    private bool isTurnCompleted;
    private Coroutine roundRoutine;

    protected override void Awake()
    {
        base.Awake();

        humanPlayer = new HumanPlayer("Human");
        aiPlayer = new AIPlayer("AI");

        turnResolver = new TurnResolver();
        captureResolver = new CaptureResolver();
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
        yield return null;

        while (true)
        {
            yield return TurnRoutine(humanPlayer);
            yield return ResolveRoutine();
            if(IsRoundEnd())
            {
                break;
            }
            yield return TurnRoutine(aiPlayer);
            yield return ResolveRoutine();
            if (IsRoundEnd())
            {
                break;
            }
        }
    }

    private IEnumerator TurnRoutine(Player player)
    {
        currentTurnPlayer = player;

        isTurnCompleted = false;

        ChangeState(player is HumanPlayer ? RoundState.PlayerTrun : RoundState.OpponentTrun);
        OnTurnStarted?.Invoke(player);
        turnResolver.ExecuteTurn(player);

        while (!isTurnCompleted)
        {
            yield return null;
        }

        OnTurnEnded?.Invoke(player);
    }
    
    public void CompleteTurn()
    {
        isTurnCompleted = true;
    }

    private IEnumerator ResolveRoutine()
    {
        ChangeState(RoundState.Resolve);
        captureResolver.Resolve(currentTurnPlayer, currentTurnPlayer.PlayedCard, tableCards);
        yield return null;
    }


    private void ChangeState(RoundState next)
    {
        CurrentState = next;
        Debug.Log($"[RoundManager] State -> {next}");
        OnRoundStateChanged?.Invoke(next);
    }

    private bool IsRoundEnd()
    {
        // 고 / 스톱 / 종료 조건
        return false;
    }

}
