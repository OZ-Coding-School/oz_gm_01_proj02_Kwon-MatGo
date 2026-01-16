using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class CapturedCardManager : Singleton<CapturedCardManager>
{
    protected override bool IsDontDestroyOnLoad => false;

    [Header("Human Areas")]
    [SerializeField] private RectTransform humanGwangArea;
    [SerializeField] private RectTransform humanTtiArea;
    [SerializeField] private RectTransform humanPeeArea;
    [SerializeField] private RectTransform humanEndArea;

    [Header("AI Areas")]
    [SerializeField] private RectTransform aiGwangArea;
    [SerializeField] private RectTransform aiTtiArea;
    [SerializeField] private RectTransform aiPeeArea;
    [SerializeField] private RectTransform aiEndArea;

    [SerializeField] private Vector2 cardSize = new Vector2(60, 90);
    [SerializeField] private Vector2 spacing = new Vector2(8, 10);

    public void RefreshCaptured(Player owner)
    {
        if (owner == null) return;

        if (owner is HumanPlayer)
        {
            RefreshCapturedInternal(
                owner.CapturedCards,
                humanGwangArea, humanTtiArea, humanEndArea, humanPeeArea);
        }
        else if (owner is AIPlayer)
        {
            RefreshCapturedInternal(
                owner.CapturedCards,
                aiGwangArea, aiTtiArea, aiEndArea, aiPeeArea);
        }
    }

    private void RefreshCapturedInternal(List<CardData> captured,RectTransform gwangArea,RectTransform ttiArea,RectTransform endArea,RectTransform peeArea)
    {
        ClearArea(gwangArea, ttiArea, endArea, peeArea);

        List<CardData> gwangs = new();
        List<CardData> ttis = new();
        List<CardData> pees = new();
        List<CardData> ends = new();

        foreach (var card in captured)
        {
            switch (card.type)
            {
                case CardType.GWANG: gwangs.Add(card); break;
                case CardType.TTI: ttis.Add(card); break;
                case CardType.END: ends.Add(card); break;
                case CardType.PEE: pees.Add(card); break;
            }
        }

        LayoutGwang(gwangs, gwangArea);
        LayoutFivePerRow(ttis, ttiArea);
        LayoutFivePerRow(ends, endArea);
        LayoutPee(pees, peeArea);
    }

    private void LayoutGwang(List<CardData> cards, RectTransform parent)
    {
        for(int i = 0; i < cards.Count; i++)
        {
            var view = CreateCapturedCard(cards[i], parent);
            SetPosition(view, row: 0, col: i);
        }
    }

    private void LayoutFivePerRow(List<CardData> cards, RectTransform parent)
    {
        for(int i = 0; i < cards.Count; i++)
        {
            int row = i / 5;
            int col = i % 5;

            var view = CreateCapturedCard(cards[i], parent);
            SetPosition(view, row, col);
        }
    }

    private void LayoutPee(List<CardData> cards, RectTransform parent)
    {
        int row = 0;
        int col = 0;

        foreach(var card in cards)
        {
            int occupy = card.isSsangPEE ? 2 : 1;

            if(col+occupy >5)
            {
                row++;
                col = 0;
            }

            var view = CreateCapturedCard(card, parent);
            SetPosition(view, row, col);

            col += occupy;
        }
    }

    private CardView CreateCapturedCard(CardData data, RectTransform parent)
    {
        var view = CardViewManager.Instance.GetCard(data, CardAreaType.HumanCapturedCard, true);

        view.transform.SetParent(parent, false);
        view.BindHandIndex(-1, false);

        view.SetVisualScale(0.5f);

        return view;
    }
    
    private void SetPosition(CardView view, int row, int col)
    {
        var rt = view.GetComponent<RectTransform>();

        float x = col * (cardSize.x + spacing.x);
        float y = row * (cardSize.y + spacing.y);

        rt.anchoredPosition = new Vector2(x, y);
    }

    private void ClearArea(params RectTransform[] areas)
    {
        foreach (var area in areas)
        {
            for (int i = area.childCount - 1; i >= 0; i--)
            {
                Destroy(area.GetChild(i).gameObject);
            }
        }
    }
}
