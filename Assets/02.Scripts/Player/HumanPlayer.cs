using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HumanPlayer : Player
{
    //콘솔 테스트용
    public int SelectIndex { get; private set; } = -1;
    public HumanPlayer(string name) : base(name)
    {
    }

    public void SetSelectIndex(int index)
    {
        SelectIndex = index;
    }

    public CardData SelectedCardSubmit()
    {
        if (SelectIndex < 0)
        {
            return null; 
        }

        int index = SelectIndex;
        SelectIndex = -1;

        if (index >= Hand.Count)
        {
            return null;
        }

        return Hand[index];
    }
}
