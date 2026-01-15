using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image frontImage;
    [SerializeField] Image backImage;
    [SerializeField] Image highlight;

    private CardData data;

    //손패 인덱스 주입
    public int HandIndex { get; private set; } = -1;
    //클릭 가능한 카드인지 체크
    public bool IsClickable { get; private set; } = false;

    public void Init(CardData card, bool front)
    {
        data = card;
        frontImage.sprite = card.cardSprite;
        SetFront(front);
        SetHighlight(false);
    }

    //카드 앞면 뒷면
    public void SetFront(bool front)
    {
        frontImage.gameObject.SetActive(front);
        backImage.gameObject.SetActive(!front);
    }

    //하이라이트
    public void SetHighlight(bool on)
    {
        if(highlight != null)
        {
            highlight.gameObject.SetActive(on);
        }
    }
    
    // 손패에 인덱스 + 클릭 가능 여부
    public void BindHandIndex(int index, bool clickable)
    {
        HandIndex = index;
        IsClickable = clickable;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //클릭 불가(내 손패만 해야됨 다른영역 XXX)
        if(!IsClickable)
        {
            return;
        }
        //내 턴 상태일때만 클릭 가능
        if(RoundManager.Instance.CurrentState != RoundState.PlayerTrun)
        {
            return;
        }

        if(HandIndex < 0)
        {
            return;
        }

        RoundManager.Instance.SetHumanSelectIndex(HandIndex);

        //임시 : 선택했을때 시각적 하이라이트(미구현)
        SetHighlight(true);
    }
    public CardData Data => data;

}
