using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TurnResolver
{
    private readonly CaptureResolver captureResolver = new CaptureResolver();

    public void ExecuteTurn(Player player, List<CardData> tableCards)
    {
        if(player == null)
        {
            Debug.LogError("[TurnResolver] Player is null");
            return;
        }
        if(tableCards == null)
        {
            Debug.LogError("[TurnResolver] tableCards null");
            return;
        }

        bool capturedThisTurn = false;

        Debug.Log($"======== 턴 시작 : {player.Name} ========");


        //손에서 카드 선택
        CardData selected = SelectCard(player);

        if(player is HumanPlayer && selected == null)
        {
            Debug.LogWarning($"[TurnResolver] {player.Name} 입력 대기중.");
            return;
        }

        //손에서 카드 제거, PlayedCard 기록
        player.PlayCard(selected);
        Debug.Log($"[Play] {player.Name} -> {selected.DebugName}");

        //바닥과 손에서 낸 카드 판정
        if(captureResolver.Resolve(player, selected, tableCards))
        {
            capturedThisTurn = true;
        }
        player.ClearPlayedCard();

        //덱에서 카드 1장 드로우
        CardData draw = DeckManager.Instance.Draw();
        if(draw == null)
        {
            Debug.LogWarning("[TurnResolver] 덱이 비었습니다. 드로우 할 수 없음");
            Debug.Log($"----- 턴 종료 : {player.Name} -----");
            return;
        }
        Debug.Log($"[Draw] {player.Name}이 드로우 -> {draw.DebugName}");

        //바닥과 드로우 카드 판정
        if(captureResolver.Resolve(player, draw, tableCards))
        {
            capturedThisTurn = true;
        }

        //점수계산, UI갱신
        if (capturedThisTurn)
        {
            Debug.Log($"[Score/UI] {player.Name} 점수계산 + UI업데이트");
            // ScoreCalculator.Calculate(player);
            // UI.UpdateScore(player);
        }

        Debug.Log($"----- 턴 종료 : {player.Name} -----");
    }

    private CardData SelectCard(Player player)
    {
        if(player is HumanPlayer humanplayer)
        {
            return humanplayer.SelectedCardSubmit();
        }

        if(player is AIPlayer aiplayer)
        {
            return aiplayer.SelectCard();
        }

        if(player.Hand.Count == 0)
        {
            return null;
        }

        return player.Hand[0];
    }
}
