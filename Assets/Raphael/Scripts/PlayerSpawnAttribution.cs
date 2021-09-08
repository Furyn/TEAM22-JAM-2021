using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnAttribution : MonoBehaviour
{
    private PlayerInputManager inputManager;

    [SerializeField]
    private Transform SpawnPoint1;
    [SerializeField]
    private Transform SpawnPoint2;
    [SerializeField]
    private Transform SpawnPoint3;
    [SerializeField]
    private Transform SpawnPoint4;

    private int playerCount;

    void Update()
    {
        playerCount = gameObject.GetComponent<PlayerCount>().playerCount;
    }

    public void ToSpawnPoint()
    {
        if (playerCount == 0)
        {
            GameObject player1 = GameObject.Find("Chara1(Clone)");
            player1.transform.position = SpawnPoint1.position;
        }

        else if (playerCount == 1)
        {
            GameObject player2 = GameObject.Find("Chara2(Clone)");
            player2.transform.position = SpawnPoint2.position;
        }

        else if (playerCount == 2)
        {
            GameObject player3 = GameObject.Find("Chara3(Clone)");
            player3.transform.position = SpawnPoint3.position;
        }

        else if (playerCount == 3)
        {
            GameObject player4 = GameObject.Find("Chara4(Clone)");
            player4.transform.position = SpawnPoint4.position;
        }
    }

}
