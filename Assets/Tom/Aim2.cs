using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim2: MonoBehaviour
{
    #region Variables

    private Vector2 lookInput = Vector2.zero;

    [SerializeField] private float mooveSpeed;
    [SerializeField] private GameObject sight;

    private Rigidbody2D rb;

    public List<GameObject> targets;
    private GameObject focusedTarget;
    #endregion

    private void Start()
    {
        sight = Instantiate(sight, new Vector3(transform.position.x, 1, transform.position.z), sight.transform.rotation);
        rb = sight.GetComponent<Rigidbody2D>();
        Sight sightScript = sight.GetComponent<Sight>();
        sightScript.makerScript = this;

    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {

    }

    void Update()
    {
        Vector3 lookDirection = new Vector3(lookInput.x, 0, lookInput.y);
        /*rb.velocity = lookDirection.normalized * mooveSpeed;*/
        sight.transform.position +=  lookDirection * 0.01f * mooveSpeed;

        float distance = float.PositiveInfinity;
        GameObject temporaryTarget = null;

        foreach (GameObject target in targets)
        {
            float targetDistance = Vector3.Distance(sight.transform.position, target.transform.position);
            if (targetDistance < distance)
            {
                temporaryTarget = target;
            }
        }

        if (temporaryTarget != focusedTarget)
        {
            focusedTarget = temporaryTarget;
        }

        Debug.Log(focusedTarget);
    }
}
