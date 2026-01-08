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
        int index = SelectIndex;
        SelectIndex = -1;

        if(Hand.Count == 0)
        {
            return null;
        }

        if(index < 0 || index >= Hand.Count)
        {
            return Hand[0];
        }

        return Hand[index];
    }
}
