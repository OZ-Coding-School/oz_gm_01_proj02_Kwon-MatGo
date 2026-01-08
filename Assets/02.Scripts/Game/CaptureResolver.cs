using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CaptureResolver
{
    public bool Resolve(Player player,
        CardData playedCard, // 낸 카드
        List<CardData> tableCards) // 바닥 카드
    {
        if(player == null || playedCard == null || tableCards == null)
        {
            Debug.LogError("[CaptureResolver] 오류 발생 확인 요망");
            return false;
        }

        //같은 월 카드 찾기
        List<CardData> sameMonthCards = tableCards.FindAll(c => c != null && c.month == playedCard.month);

        //못먹는 경우
        if(sameMonthCards.Count == 0)
        {
            tableCards.Add(playedCard);
            Debug.Log($"[Capture] 맞는 카드가 없음. {playedCard.name} 바닥에 놓음");
            return false;
        }

        //먹는 경우
        player.AddCapturedCard(playedCard);
        
        foreach(var card in sameMonthCards)
        {
            player.AddCapturedCard(card);
            tableCards.Remove(card);
        }
        return true;
    }
}
