using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    [SerializeField] private float bleedingTime = 5;
    private bool isBleeding = false;

    private void Update()
    {
        if (!isBleeding)
        {
            StartCoroutine(Bleeding());
            isBleeding = true;
        }
    }

    IEnumerator Bleeding()
    {
        yield return new WaitForSeconds(bleedingTime);
        isBleeding = false;
        gameObject.SetActive(false);
        StopCoroutine(Bleeding());
    }
}
