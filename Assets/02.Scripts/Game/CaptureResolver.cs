using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CaptureResult
{ 
    None,       //못먹음
    Capture,    //먹음
    TwoChoice   //먹을거 여러 개(임시로 선언)
}

public sealed class CaptureResolver
{
    public CaptureResult Resolve(Player player,
        CardData playedCard, // 낸 카드
        List<CardData> tableCards) // 바닥 카드
    {
        if(player == null || playedCard == null || tableCards == null)
        {
            Debug.LogError("[CaptureResolver] 오류 발생 확인 요망");
            return CaptureResult.None;
        }

        //같은 월 카드 찾기
        List<CardData> sameMonthCards = GetSameMonthCards(playedCard.month, tableCards);

        //못먹는 경우
        if(sameMonthCards.Count == 0)
        {
            tableCards.Add(playedCard);
            Debug.Log($"[Capture] 맞는 카드가 없음. {playedCard.name} 바닥에 놓음");
            return CaptureResult.None;
        }

        //먹는 경우
        player.AddCapturedCard(playedCard);

        foreach(var card in sameMonthCards)
        {
            player.AddCapturedCard(card);
            tableCards.Remove(card);
        }
        return sameMonthCards.Count == 1 ? CaptureResult.Capture : CaptureResult.TwoChoice;
    }

    private List<CardData> GetSameMonthCards(CardMonth month, List<CardData> tableCards)
    {
        List<CardData> result = new List<CardData>();

        foreach (var card in tableCards)
        {
            if(card.month == month)
            {
                result.Add(card);
            }
        }

        return result;
    }
}
