using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CaptureResult
{
    public bool captured;
    public List<CardData> capturedCards;

    public static CaptureResult None => new CaptureResult { captured = false, capturedCards = null };
}


public sealed class CaptureResolver
{
    public CaptureResult Resolve(Player player,
        CardData playedCard, // 낸 카드
        List<CardData> tableCards) // 바닥 카드
    {
        if(player == null || playedCard == null || tableCards == null)
        {
            return CaptureResult.None;
        }

        //같은 월 카드 찾기
        List<CardData> sameMonthCards = tableCards.FindAll(c => c != null && c.month == playedCard.month);

        //못먹는 경우
        if(sameMonthCards.Count == 0)
        {
            tableCards.Add(playedCard);
            return CaptureResult.None;
        }

        //먹는 경우
        var capturedList = new List<CardData>();

        player.AddCapturedCard(playedCard);
        capturedList.Add(playedCard);

        foreach (var card in sameMonthCards)
        {
            player.AddCapturedCard(card);
            capturedList.Add(card);
            tableCards.Remove(card);
        }
        return new CaptureResult
        {
            captured = true,
            capturedCards = capturedList
        };
    }
}
