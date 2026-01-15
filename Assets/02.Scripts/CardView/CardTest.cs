using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    [SerializeField] private CardView cardViewPrefab;
    [SerializeField] private CardData testCard;

    void Start()
    {
        CardView cv = Instantiate(cardViewPrefab, transform);
        cv.Init(testCard, front: true);

        // 1초 뒤 뒷면 테스트
        Invoke(nameof(Flip), 1f);
    }

    void Flip()
    {
        GetComponentInChildren<CardView>().SetFront(false);
    }
}
