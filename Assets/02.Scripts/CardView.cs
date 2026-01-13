using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] Image frontImage;
    [SerializeField] Image backImage;
    [SerializeField] Image highlight;

    private CardData data;

    public void Init(CardData card, bool front)
    {
        data = card;
        frontImage.sprite = card.cardSprite;
        SetFront(front);
        SetHighlight(false);
    }

    public void SetFront(bool front)
    {
        frontImage.gameObject.SetActive(front);
        backImage.gameObject.SetActive(!front);
    }

    public void SetHighlight(bool on)
    {
        if(highlight != null)
        {
            highlight.gameObject.SetActive(on);
        }
    }
    public CardData Data => data;

}
