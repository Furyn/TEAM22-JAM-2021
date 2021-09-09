using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRotation : MonoBehaviour
{
    void Update()
    {
        transform.RotateAround(transform.position, transform.up, Time.unscaledDeltaTime * 90f);
    }
}
