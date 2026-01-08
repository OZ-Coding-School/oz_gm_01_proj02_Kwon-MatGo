using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AIPlayer : Player
{
    public AIPlayer(string name) : base(name)
    {
    }

    public CardData SelectCard()
    {
        if(Hand.Count == 0)
        {
            return null;
        }
        return Hand[0];
    }
}
