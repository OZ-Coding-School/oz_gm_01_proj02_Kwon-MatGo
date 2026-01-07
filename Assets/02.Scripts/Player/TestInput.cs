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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("[TestInput] Space pressed -> Turn Complete");
            RoundManager.Instance.CompleteTurn();
        }
    }
}