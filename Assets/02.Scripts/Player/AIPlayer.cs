using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AIPlayer : Player
{
    public AIPlayer(string name) : base(name)
    {
    }

    public override void OnTurnStarted()
    {
        CardData card = DeckManager.Instance.Draw();
        SetPlayedCard(card);

        Debug.Log($"[{Name}] played {card.name}");

        RoundManager.Instance.CompleteTurn();
    }
    public override void OnTurnEnded()
    {
        Debug.Log($"[AIPlayer] {Name} 턴 종료");
    }

    private void ExecuteAIAction()
    {
        //판단 로직 설정해야함
        Debug.Log($"[AIPlayer]{Name} 행동 결정 완료");

        RoundManager.Instance.CompleteTurn();
    }
}
