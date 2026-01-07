using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TurnResolver
{
    public void ExecuteTurn(Player player)
    {
        if(player == null)
        {
            Debug.LogError("[TurnResolver] Player is null");
            return;
        }

        Debug.Log($"[TurnREsolver] Execute Turn : {player.Name}");

        player.OnTurnStarted();
    }
}
