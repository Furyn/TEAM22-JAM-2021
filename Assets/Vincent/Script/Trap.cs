using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    [SerializeField] private float durationBlood = 0.5f;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            ActionTrap action = other.GetComponent<ActionTrap>();
            if (action != null)
            {
                Debug.Log("Active blood");
                action.ActiveBlood(durationBlood);
            }
        }
    }
}
