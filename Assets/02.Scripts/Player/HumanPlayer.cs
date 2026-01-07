using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HumanPlayer : Player
{
    private bool isMyTurn;

    public HumanPlayer(string name) : base(name)
    {
    }

    public override void OnTurnStarted()
    {
        //랜덤 드로우
        CardData card = DeckManager.Instance.Draw();
        SetPlayedCard(card);

        Debug.Log($"[{Name}] played {card.name}");

        RoundManager.Instance.CompleteTurn();
    }

    public override void OnTurnEnded()
    {
        isMyTurn= false;
        Debug.Log($"[HumanPlayer] {Name} 턴 종료");

        //UI 입력 비활성화
    }

    public void CommitAction()
    {
        if(!isMyTurn)
        {
            return;
        }

        Debug.Log($"[HumanPlayer]{Name} 행동 결정 완료");

        isMyTurn = false;
        RoundManager.Instance.CompleteTurn();
    }
}
