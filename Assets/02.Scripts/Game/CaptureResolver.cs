using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CaptureResolver
{
    public bool Resolve(Player player,
        CardData playedCard, // ³½ Ä«µå
        List<CardData> tableCards) // ¹Ù´Ú Ä«µå
    {
        if(player == null || playedCard == null || tableCards == null)
        {
            Debug.LogError("[CaptureResolver] ¿À·ù ¹ß»ý È®ÀÎ ¿ä¸Á");
            return false;
        }

        //°°Àº ¿ù Ä«µå Ã£±â
        List<CardData> sameMonthCards = tableCards.FindAll(c => c != null && c.month == playedCard.month);

        //¸ø¸Ô´Â °æ¿ì
        if(sameMonthCards.Count == 0)
        {
            tableCards.Add(playedCard);
            Debug.Log($"[Capture] ¸Â´Â Ä«µå°¡ ¾øÀ½. {playedCard.name} ¹Ù´Ú¿¡ ³õÀ½");
            return false;
        }

        //¸Ô´Â °æ¿ì
        player.AddCapturedCard(playedCard);

        Debug.Log($"[Capture] {player.Name}°¡ {playedCard.DebugName}·Î Ä¸Ã³ ¹ß»ý");

        foreach (var card in sameMonthCards)
        {
            player.AddCapturedCard(card);
            tableCards.Remove(card);
            Debug.Log($"[Capture] ¡æ ¹Ù´Ú Ä«µå È¹µæ : {card.DebugName}");
        }
        Debug.Log($"[Capture] ÃÑ {sameMonthCards.Count + 1}Àå È¹µæ");
        return true;
    }
}
