using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Matgo/Card Data", fileName = "Card")]
public class CardData : ScriptableObject
{
    [Header("Card Type")]
    public CardMonth month;
    public CardType type;

    [Header("Gwang Properties")]
    public bool isBiGwang;  //비광체크

    [Header("PEE Properties")]
    public bool isSsangPEE; //쌍피 카드 체크

    [Header("TTI Properties")]
    public bool isHongDan;
    public bool isChungDan;
    public bool isChoDan;   //홍/청/초단 체크

    [Header("End Type")]
    public bool isGodori;   //고도리 체크
    public bool isGukjin;   //국진 선택 여부 체크 (쌍피? 열끗?)

    [Header("Card Image")]
    public Sprite cardSprite;

}
