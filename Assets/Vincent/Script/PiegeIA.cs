using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiegeIA : MonoBehaviour
{

    private List<AIBehaviour> allIA = new List<AIBehaviour>(); 

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC")
        {
            AIBehaviour ia = other.GetComponent<AIBehaviour>();

            if (ia)
            {
                allIA.Add(ia);
                ia.SetDestination(gameObject.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC")
        {
            AIBehaviour ia = other.GetComponent<AIBehaviour>();

            if (ia)
            {
                ia.SetDestination(new Vector3());
            }
        }
    }

    private void OnDisable()
    {
        foreach (AIBehaviour ia in allIA)
        {
            ia.SetDestination(Vector3.zero);
        }
    }

}
