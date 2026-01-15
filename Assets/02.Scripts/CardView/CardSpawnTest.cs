using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnTest : MonoBehaviour
{
    [SerializeField] private CardData testCard;

    void Start()
    {
        // 내 손패에 카드 1장 생성
        CardViewManager.Instance.GetCard(
            testCard,
            CardAreaType.HumanHandCard,
            true
        );
    }
}
