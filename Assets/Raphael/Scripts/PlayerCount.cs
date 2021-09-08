using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCount : MonoBehaviour
{
    [HideInInspector]
    public int playerCount = 0;
    public void AddPlayer()
    {
        playerCount++;
        Debug.Log("Player " + playerCount + " joined the game.");
    }

}
