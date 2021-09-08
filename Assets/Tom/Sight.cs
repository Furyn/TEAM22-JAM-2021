using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    [HideInInspector] public Aim2 makerScript;

    private void OnTriggerEnter(Collider collision)
    {
        GameObject collidingObject = collision.gameObject;
        if (collidingObject && collidingObject.tag == "Player" | collidingObject.tag == "NPC")
        {
            makerScript.targets.Add(collidingObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        GameObject collidingObject = collision.gameObject;
        if (collidingObject.tag == "Player" | collidingObject.tag == "NPC")
        {
            makerScript.targets.Remove(collidingObject);
        }
    }
}
