using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Lobby,
    InGame,
    Result
}


public class GameManager : Singleton<GameManager>
{
    protected override bool IsDontDestroyOnLoad => true;

    public static event Action<GameState> OnGameFlowStateChanged;

    public GameState CurrentState { get; private set; }

    [SerializeField] private RoundManager roundManager;

    protected override void Awake()
    {
        base.Awake();
        ChangeState(GameState.Lobby);
    }

    public void StartGame()
    {
        ChangeState(GameState.InGame);
        roundManager.StartRound();
    }

    public void OnRoundEnded()
    {
        ChangeState(GameState.Result);
    }

    private void ChangeState(GameState next)
    {
        CurrentState = next;
        Debug.Log($"[GameManager] GameFlow -> {next}");
        OnGameFlowStateChanged?.Invoke(next);
    }
}
