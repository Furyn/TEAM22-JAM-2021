using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    [SerializeField] private float speed;

    public GameObject owner;

    void Update()
    {
        transform.transform.position = transform.position + transform.forward * speed;
    }
}
