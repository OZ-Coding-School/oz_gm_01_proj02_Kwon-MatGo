using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CardAreaType
{
    AIHandCard,
    AICapturedCard,
    Deck,
    HumanHandCard,
    HumanCapturedCard,
    TableCard
}


public class CardViewManager : MonoBehaviour
{
    public static CardViewManager Instance { get; private set; }

    [SerializeField] private CardView cardViewPrefab;

    [SerializeField] private Transform AIHandCardArea;      //상대 손
    [SerializeField] private Transform AICapturedCardArea;  //상대 먹은
    [SerializeField] private Transform DeckArea;            //덱
    [SerializeField] private Transform HumanHandCardArea;   //사람 손
    [SerializeField] private Transform HumanCapturedCardArea;   //사람 먹은
    [SerializeField] private Transform TableCardArea;       //바닥카드

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public CardView CreateCard(CardData data, CardAreaType area, bool front)
    {
        Transform parent = GetParent(area);
        if(parent == null)
        {
            Debug.LogError($"[CardViewManager] 부모가 null임");
            return null;
        }

        CardView view = Instantiate(cardViewPrefab, parent);
        view.Init(data, front);

        return view;
    }

    public void ClearArea(CardAreaType area)
    {
        Transform parent = GetParent(area);
        if(parent == null)
        {
            return;
        }

        for(int i = parent.childCount -1; i>=0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    private Transform GetParent(CardAreaType area)
    {
        switch (area)
        {
            case CardAreaType.AIHandCard:
                return AIHandCardArea;
            case CardAreaType.AICapturedCard:
                return AICapturedCardArea;
            case CardAreaType.Deck:
                return DeckArea;
            case CardAreaType.HumanHandCard:
                return HumanHandCardArea;
            case CardAreaType.HumanCapturedCard:
                return HumanCapturedCardArea;
            case CardAreaType.TableCard:
                return TableCardArea;
            default:
                return null;
        }
    }
}
