using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player
{
    public string Name { get; protected set; }

    //플레이어가 먹은 카드 저장 컨테이너(초기 슬롯 갯수 32개로 정함)
    //보통 그냥 30장 먹기 전에 끝나니깐..
    private const int MaxCapturedCards = 32;
    //플레이어가 손에 들고 있을 카드 슬롯
    private const int MaxHandCards = 10;
    public List<CardData> CapturedCards { get; } = new List<CardData>(MaxCapturedCards);
    public List<CardData> Hand { get; } = new List<CardData>(MaxHandCards);
    public CardData PlayedCard { get; protected set; } //이번턴에 낸 카드 받아옴
    
    protected Player(string name)
    {
        Name = name;
    }
    
    public void PlayCard(CardData card)
    {
        Hand.Remove(card);
        PlayedCard = card;
    }
    
    public void ClearPlayedCard()
    {
        PlayedCard = null;
    }

    public void AddCapturedCard(CardData card)
    {
        if(card == null)
        {
            return;
        }
        CapturedCards.Add(card);
    }
}
