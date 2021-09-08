using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    #region Variables

    private Vector2 aimDir;

    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject bolt;

    [SerializeField] private float arrowDistance;
    [SerializeField] private float arrowFadeDuration;

    [SerializeField] private float aimCooldown;

    private bool isAiming = false;
    private bool canAim = true;
    private bool canShoot = false;
    #endregion

    private void Start()
    {
        if(arrow.activeInHierarchy)
        {
            arrow.SetActive(false);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        aimDir = context.ReadValue<Vector2>();
    }

    public void OnShoot()
    {
        if (canShoot)
        {
            Shoot();
        }
        else
        {
            Debug.Log("on cooldown");
        }
    }

    void Update()
    {
        IsAiming();

        if (canShoot)
        {
            Vector3 direction = new Vector3(aimDir.x, 0, aimDir.y);
            arrow.transform.localPosition = (Mathf.Clamp(direction.magnitude, 0f, 1f) * direction.normalized) * arrowDistance;
        }
    }

    private void IsAiming()
    {
        if (aimDir != Vector2.zero)
        {
            if (!isAiming)
            {
                if (canAim)
                {
                    arrow.SetActive(true);
                    canShoot = true;
                }
                else
                {
                    Debug.Log("nope");
                }
                isAiming = true;
            }
        }
        else
        {
            if (isAiming)
            {
                if (canAim)
                {
                    StartCoroutine("AimCooldown");
                }
                isAiming = false;
            }
        }
    }

    private void Shoot()
    {
        GameObject newBolt = Instantiate(bolt,transform.position, bolt.transform.rotation, null);
        newBolt.transform.forward = new Vector3(aimDir.x, 0, aimDir.y);

        Bolt newBoltScript = newBolt.GetComponent<Bolt>();
        newBoltScript.owner = gameObject;

        StartCoroutine("AimCooldown");
    }

    IEnumerable AimCooldown()
    {
        canAim = false;
        arrow.SetActive(false);
        canShoot = false;

        for (float f = 0; f < aimCooldown; f += Time.deltaTime)
        {
            if (f >= arrowFadeDuration && f < (aimCooldown - arrowFadeDuration))
            {
                arrow.SetActive(false);
            }
            else if(f >= (aimCooldown - arrowFadeDuration))
            {
                canAim = true;
                Debug.Log("ok");

                StopCoroutine("AimCooldown");
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
