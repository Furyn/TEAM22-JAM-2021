using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMesh : MonoBehaviour
{
    private GameObject PlayerManager;
    void Start()
    {
        int playerCount = GameObject.Find("PlayerManager").GetComponent<PlayerCount>().playerCount;

        if (playerCount == 1)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.magenta;
        }

        else if (playerCount == 2)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
        }

        else if (playerCount == 2)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }

        else if (playerCount == 2)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }

}
