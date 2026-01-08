using UnityEngine;

public class TestInput : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("[TestInput] Enter pressed -> StartGame");
            GameManager.Instance.StartGame();
        }

        //임시 카드 선택 분기 반드시 교체
        if (Input.GetKeyDown(KeyCode.Alpha0)) RoundManager.Instance.SetHumanSelectIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha1)) RoundManager.Instance.SetHumanSelectIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) RoundManager.Instance.SetHumanSelectIndex(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) RoundManager.Instance.SetHumanSelectIndex(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) RoundManager.Instance.SetHumanSelectIndex(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) RoundManager.Instance.SetHumanSelectIndex(5);
        if (Input.GetKeyDown(KeyCode.Alpha6)) RoundManager.Instance.SetHumanSelectIndex(6);
        if (Input.GetKeyDown(KeyCode.Alpha7)) RoundManager.Instance.SetHumanSelectIndex(7);
        if (Input.GetKeyDown(KeyCode.Alpha8)) RoundManager.Instance.SetHumanSelectIndex(8);
        if (Input.GetKeyDown(KeyCode.Alpha9)) RoundManager.Instance.SetHumanSelectIndex(9);
    }
}