using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator Animator;

    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float gravityValue = -100f;
    [SerializeField]
    private float rotationSpeed = 1440f;
    [SerializeField]
    private float TimeBetweenPausePress = 0.2f;
    private bool pauseCooldown = false;

    private Vector2 movementInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;
    private bool isGrounded = false;


    private void Start()
    {
        Animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnPause()
    {
        if (!pauseCooldown)
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject pausemenu = canvas.transform.GetChild(0).gameObject;

            if (Time.timeScale == 0f && pausemenu.activeSelf)
            {
                pausemenu.SetActive(false);
                Time.timeScale = 1f;
            }

            else if (Time.timeScale == 1f && !pausemenu.activeSelf)
            {
                Time.timeScale = 0f;
                pausemenu.SetActive(true);
            }

            pauseCooldown = true;
            StartCoroutine(PauseCooldown());
        }
        
    }

    IEnumerator PauseCooldown()
    {
        yield return new WaitForSecondsRealtime(TimeBetweenPausePress);
        pauseCooldown = false;
    }

    void Update()
    {
        if (!isGrounded)
        {
            Vector2 gravity = new Vector3(0, gravityValue, 0);
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        rb.velocity = move.normalized * playerSpeed;

        Vector3 look = new Vector3(lookInput.x, 0, lookInput.y);

        if (move != Vector3.zero && look == Vector3.zero)
        {
            UnityEngine.Quaternion toRotation = UnityEngine.Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = UnityEngine.Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        else if (move != Vector3.zero && look != Vector3.zero)
        {
            UnityEngine.Quaternion toRotation = UnityEngine.Quaternion.LookRotation(look.normalized, Vector3.up);
            transform.rotation = UnityEngine.Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        else if (move == Vector3.zero && look != Vector3.zero)
        {
            UnityEngine.Quaternion toRotation = UnityEngine.Quaternion.LookRotation(look.normalized, Vector3.up);
            transform.rotation = UnityEngine.Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        Animator.SetFloat("Speed", (Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z)));

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = false;
        }
    }
}