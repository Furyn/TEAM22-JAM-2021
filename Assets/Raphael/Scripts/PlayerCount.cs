using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCount : MonoBehaviour
{
    private PlayerInputManager inputManager;

    public GameObject playerPrefabB;
    [SerializeField] private Color p2SightColor;

    public GameObject playerPrefabC;
    [SerializeField] private Color p3SightColor;

    public GameObject playerPrefabD;
    [SerializeField] private Color p4SightColor;

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
            inputManager.playerPrefab.GetComponent<Aim2>().sightColor = p2SightColor;
        }

        else if (playerCount == 2)
        {
            inputManager.playerPrefab = playerPrefabC;
            inputManager.playerPrefab.GetComponent<Aim2>().sightColor = p3SightColor;
        }

        else if (playerCount == 3)
        {
            inputManager.playerPrefab = playerPrefabD;
            inputManager.playerPrefab.GetComponent<Aim2>().sightColor = p4SightColor;
        }
    }

}
