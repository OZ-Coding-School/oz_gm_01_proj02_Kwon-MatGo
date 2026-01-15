using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFanLayOut : MonoBehaviour
{
    [SerializeField] private float radius = 400f;
    [SerializeField] private float fanAngle = 40f;
    [SerializeField] private float yOffset = 0;

    public void Arrange(List<Transform> cards)
    {
        int count = cards.Count;
        if(count == 0)
        {
            return;
        }

        float startAngle = -fanAngle * 0.5f;
        float step = (count == 1) ? 0f : fanAngle / (count - 1);

        for(int i = 0; i < count; i++)
        {
            float angle = startAngle + step * i;

            RectTransform rt = cards[i] as RectTransform;
            if (rt == null)
            {
                continue;
            }

            //위치 회전 초기화
            rt.localScale = Vector3.one;
            rt.localRotation = Quaternion.identity;
            rt.anchoredPosition = Vector2.zero;

            Vector2 pos = (Quaternion.Euler(0, 0, angle) * Vector2.up * radius);

            pos.y += yOffset;

            rt.anchoredPosition = pos;
            rt.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
