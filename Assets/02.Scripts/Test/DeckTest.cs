using System.Collections.Generic;
using UnityEngine;

public class DeckTest : MonoBehaviour
{
    [SerializeField] private bool runOnStart = true;

    private void Start()
    {
        if (!runOnStart) return;

        RunDeckTest();
    }

    [ContextMenu("Run Deck Test")]
    public void RunDeckTest()
    {
        var deck = DeckManager.Instance;
        if (deck == null)
        {
            Debug.LogError("[DeckTestRunner] DeckManager.Instance가 null 입니다. 씬에 DeckManager를 붙인 GameObject가 있어야 합니다.");
            return;
        }

        deck.InitializeDeck(shuffle: true);
        Debug.Log($"[TEST] Deck initialized. Count = {deck.Count}");

        // 48장 중복 없이 드로우 되는지 검증
        HashSet<CardData> drawnSet = new HashSet<CardData>();
        Dictionary<CardMonth, int> monthCount = new Dictionary<CardMonth, int>();
        Dictionary<CardType, int> typeCount = new Dictionary<CardType, int>();

        int drawn = 0;
        while (!deck.IsEmpty())
        {
            var card = deck.Draw();
            drawn++;

            if (card == null)
            {
                Debug.LogError($"[TEST] Draw returned null at draw #{drawn}");
                continue;
            }

            if (!drawnSet.Add(card))
            {
                Debug.LogError($"[TEST] Duplicate draw detected: {card.name} / {card.DebugName}");
            }

            monthCount.TryGetValue(card.month, out int mc);
            monthCount[card.month] = mc + 1;

            typeCount.TryGetValue(card.type, out int tc);
            typeCount[card.type] = tc + 1;

            // 로그는 너무 많으면 지저분하니 옵션으로 줄이거나, 처음 몇 장만 찍어도 됨
            Debug.Log($"[DRAW {drawn:00}] {card.DebugName}");
        }

        Debug.Log($"[TEST] Total drawn = {drawn} (expected 48)");
        foreach (CardMonth m in System.Enum.GetValues(typeof(CardMonth)))
        {
            monthCount.TryGetValue(m, out int cnt);
            Debug.Log($"[TEST] {m} count = {cnt} (expected 4)");
        }

        foreach (CardType t in System.Enum.GetValues(typeof(CardType)))
        {
            typeCount.TryGetValue(t, out int cnt);
            Debug.Log($"[TEST] Type {t} count = {cnt}");
        }

        Debug.Log("[TEST] Deck test finished.");
    }
}