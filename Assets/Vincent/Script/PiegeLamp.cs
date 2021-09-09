using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiegeLamp : MonoBehaviour
{

    private List<GameObject> allPlayer = new List<GameObject>(); 

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AIBehaviour ia = other.GetComponent<AIBehaviour>();

            if (ia)
            {
                allPlayer.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            allPlayer.Remove(other.gameObject);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject player in allPlayer)
        {
            
        }
    }

}
