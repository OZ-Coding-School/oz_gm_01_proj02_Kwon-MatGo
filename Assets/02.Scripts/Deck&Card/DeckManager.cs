using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    [SerializeField] private List<CardData> allCards = new List<CardData>(48);

    private Stack<CardData> drawDeck = new Stack<CardData>();
    private readonly System.Random rng = new System.Random();

    public int Count => drawDeck.Count;

    ///<summary>
    ///덱 초기화
    ///SO리스트를 기반으로 덱 구성 및 셔플
    ///</summary>
    public void InitializeDeck(bool shuffle = true)
    {
        CheckdateAllCards();

        // 리스트 복사 후 셔플(Stack으로 쌓음)
        List<CardData> temp = new List<CardData>(allCards);

        if(shuffle)
        {
            Shuffle(temp);
        }
        drawDeck.Clear();

        //스택의 맨 위가 맨 마지막에 Push된 것임.-> 뒤에서부터 Push
        for(int i = temp.Count - 1; i >= 0; i--)
        {
            drawDeck.Push(temp[i]);
        }
    }

    ///<summary>
    ///덱에서 1장 드로우
    ///</summary>
    public CardData Draw()
    {
        if(drawDeck.Count == 0)
        {
            Debug.LogWarning("[DeckManager] 덱이 비어 드로우 할 수 없음");
            return null;
        }
        return drawDeck.Pop();
    }

    public bool IsEmpty() => drawDeck.Count == 0;

    private void CheckdateAllCards()
    {
        if (allCards == null || allCards.Count != 48)
        {
            Debug.LogError($"[DeckManager] 카드의 갯수가 48개가 아님.");
        }
        
        // null체크/중복 체크
        HashSet<CardData> set = new HashSet<CardData>();
        for(int i = 0; i < allCards.Count; i++)
        {
            var card = allCards[i];
            if(card == null)
            {
                Debug.LogError($"[DeckManager] allCards[{i}]가 null임");
                continue;
            }
            if(!set.Add(card))
            {
                Debug.LogError($"[DeckManager] allCards에 동일한 SO가 중복포함.");
            }
        }

        //월별 4장인지 체크
        Dictionary<CardMonth, int> monthCount = new Dictionary<CardMonth, int>();
        foreach(var c in allCards)
        {
            if (c == null) continue;
            monthCount.TryGetValue(c.month, out int cnt);
            monthCount[c.month] = cnt + 1;
        }
        foreach (CardMonth m in System.Enum.GetValues(typeof(CardMonth)))
        {
            monthCount.TryGetValue(m, out int cnt);
            if(cnt != 4)
            {
                Debug.LogWarning($"[DeckManager] {m}월 카드 수가 4장이 아닙니다.");
            }
        }
    }

    private void Shuffle(List<CardData> list)
    {
        //피셔-예이츠 셔플 알고리즘으로 덱 셔플
        for(int i = list.Count -1; i>0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }

    }
}
