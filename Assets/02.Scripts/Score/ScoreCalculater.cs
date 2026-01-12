using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreCalculater
{
    public static int Calculate(Player player)
    {
        if(player == null)
        {
            return 0;
        }

        var cards = player.CapturedCards;
        int score = 0;

        score += CalculateGwang(cards);
        score += CalculateEnd(cards);
        score += CalculateTTI(cards);
        score += CalculatePEE(cards);

        return score;
    }

    //========== 광 점수 판정 ==========
    private static int CalculateGwang(List<CardData> cards)
    {
        var gwangs = cards.FindAll(card => card.type == CardType.GWANG);
        int count = gwangs.Count;

        if(count < 3)
        {
            return 0;
        }

        //비광 포함 3개는 2점
        bool hasBiGwang = gwangs.Exists(card => card.isBiGwang);

        if(count == 3)
        {
            return hasBiGwang ? 2 : 3;
        }
        if(count == 4)
        {
            return 4;
        }
        if(count >= 5)
        {
            return 15;
        }

        return 0;
    }

    //========== 열끗 점수 판정 ==========
    private static int CalculateEnd(List<CardData> cards)
    {
        var ends = cards.FindAll(card => card.type == CardType.END);
        int count = ends.Count;
        int score = 0;

        //끗 5장 기본점수 + 1점씩
        if(count >= 5)
        {
            score += count - 4;
        }

        //고도리(2,4,8월)
        bool hasGodori =
                ends.Exists(card => card.month == CardMonth.Feb) &&
                ends.Exists(card => card.month == CardMonth.Apr) &&
                ends.Exists(card => card.month == CardMonth.Aug);

        if (hasGodori)
        {
            score += 5;
        }

        return score;
    }

    //========== 띠 점수 판정 ==========
    private static int CalculateTTI(List<CardData> cards)
    {
        var ttis = cards.FindAll(card => card.type == CardType.TTI);
        int count = ttis.Count; 
        int score = 0;

        //띠 5장 기본 점수 + 1점씩
        if(count >= 5)
        {
            score += count - 4;
        }

        //홍/청/초단 점수
        if (ttis.FindAll(card => card.isHongDan).Count == 3)
        {
            score += 3;
        }
        if(ttis.FindAll(card => card.isChungDan).Count == 3)
        {
            score += 3;
        }
        if (ttis.FindAll(card => card.isChoDan).Count == 3)
        {
            score += 3;
        }

        return score;
    }

    //========== 피 점수 판정 ==========
    private static int CalculatePEE(List<CardData> cards)
    {
        int peeCount = 0;

        foreach (var card in cards)
        {
            if(card.type != CardType.PEE)
            {
                continue;
            }

            //쌍피 계산
            peeCount += card.isSsangPEE ? 2 : 1;
        }
        if(peeCount <10)
        {
            return 0;
        }

        return peeCount - 9;
    }
}
