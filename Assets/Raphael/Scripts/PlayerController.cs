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
    private float rotationSpeed = 1440f;
    [SerializeField]
    private float TimeBetweenPausePress = 0.2f;
    private bool pauseCooldown = false;

    private Vector2 movementInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;

    [HideInInspector] public int sightsNb;

    private bool imDead = false;

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
        if (!imDead)
        {
            Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
            rb.velocity = move.normalized * playerSpeed;

            if (move != Vector3.zero)
            {
                UnityEngine.Quaternion toRotation = UnityEngine.Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = UnityEngine.Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            if (Animator)
            {
                Animator.SetFloat("Speed", (Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z)));
            }
        }

        
    }

    public void DIE()
    {
        imDead = true;
        GetComponent<Aim2>().imDead = true;
        Animator.SetTrigger("Death");
        StartCoroutine(WaitForDeathAnim(3));
    }

    IEnumerator WaitForDeathAnim(float time)
    {
        yield return new WaitForSeconds(time);
        transform.position = new Vector3(transform.position.x, transform.position.y - 50, transform.position.z);
    }
}