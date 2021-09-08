using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCount : MonoBehaviour
{
    private PlayerInputManager inputManager;
    public GameObject playerPrefabB;
    public GameObject playerPrefabC;
    public GameObject playerPrefabD;

    [HideInInspector]
    public int playerCount = 0;

    void Start()
    {
        inputManager = gameObject.GetComponent<PlayerInputManager>();
    }

    public void AddPlayer()
    {
        playerCount++;
        Debug.Log("Player " + playerCount + " joined the game.");

        if (playerCount == 1)
        {
            inputManager.playerPrefab = playerPrefabB;
        }

        else if (playerCount == 2)
        {
            inputManager.playerPrefab = playerPrefabC;
        }

        else if (playerCount == 3)
        {
            inputManager.playerPrefab = playerPrefabD;
        }
    }

}
