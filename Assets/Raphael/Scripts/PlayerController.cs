using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float speed;
    [SerializeField] private float movementRotationSpeed;
    [SerializeField] private float lookRotationSpeed;

    [SerializeField] private int playerID = 0;
    [SerializeField] private Player player;


    private void Start()
    {
        /*player = Rewired.ReInput.players.GetPlayer(playerID);*/
    }


    // Update is called once per frame
    private void Update()
    {
        if (player == null)
        {
            Debug.Log("NOT SETUP CONTROLLER");
            return;
        }

        float moveX = player.GetAxis("Move X");
        float moveZ = player.GetAxis("Move Z");

        Vector3 movement = new Vector3(moveX, 0, moveZ);
        rb.velocity = movement.normalized * speed;

        float lookX = player.GetAxis("Look X");
        float lookZ = player.GetAxis("Look Z");

        Vector3 lookDirection = new Vector3(lookX, 0, lookZ);
        lookDirection = lookDirection.normalized;

        if (movement != Vector3.zero && lookDirection == Vector3.zero)
        {
            UnityEngine.Quaternion toRotation = UnityEngine.Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = UnityEngine.Quaternion.RotateTowards(transform.rotation, toRotation, movementRotationSpeed * Time.deltaTime);
        }

        else if (movement == Vector3.zero && lookDirection != Vector3.zero)
        {
            UnityEngine.Quaternion toRotation = UnityEngine.Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = UnityEngine.Quaternion.RotateTowards(transform.rotation, toRotation, lookRotationSpeed * Time.deltaTime);
        }

        else if (movement != Vector3.zero && lookDirection != Vector3.zero)
        {
            UnityEngine.Quaternion toRotation = UnityEngine.Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = UnityEngine.Quaternion.RotateTowards(transform.rotation, toRotation, lookRotationSpeed * Time.deltaTime);
        }
    }
}
