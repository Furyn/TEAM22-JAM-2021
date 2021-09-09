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

    [HideInInspector] public int pNb = 1;
    [SerializeField] private int playerKillScore;
    public Color sightColor;
    private GameManager gameManager;

    public List<GameObject> targets;
    private GameObject focusedTarget;

    private bool onCD = false;
    [SerializeField] private float cooldown;
    public Vector3 spawnPoint;
    #endregion

    private void Start()
    {
        spawnPoint = new Vector3(transform.position.x, 1, transform.position.z);

        sight = Instantiate(sight, spawnPoint, sight.transform.rotation);

        Sight sightScript = sight.GetComponent<Sight>();
        sightScript.makerScript = this;

        sight.GetComponent<SpriteRenderer>().color = sightColor;

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if(!onCD && focusedTarget)
        {
            Debug.Log("boom");

            if(focusedTarget.tag == "player")
            {
                int score = (int)gameManager.GetType().GetField("p" + pNb + "Score").GetValue(gameManager);
                gameManager.GetType().GetField("p" + pNb + "Score").SetValue(gameManager, score + playerKillScore);
                score = (int)gameManager.GetType().GetField("p" + pNb + "Score").GetValue(gameManager);
                Debug.Log("Player " + pNb + " score: " + score);
            }
            else if(focusedTarget.tag == "NPC")
            {
                int score = (int)gameManager.GetType().GetField("p" + pNb + "Score").GetValue(gameManager);
                gameManager.GetType().GetField("p" + pNb + "Score").SetValue(gameManager, score + playerKillScore);
                score = (int)gameManager.GetType().GetField("p" + pNb + "Score").GetValue(gameManager);
                Debug.Log("Player " + pNb + " score: " + score);
            }

            sight.transform.position = spawnPoint;

            onCD = true;
            StartCoroutine(Cooldown(cooldown));
        }
        else if (onCD)
        {
            Debug.Log("onCooldown");
        }
    }

    void Update()
    {
        Vector3 lookDirection = new Vector3(lookInput.x, 0, lookInput.y);

        if (!onCD)
        {
            sight.transform.position += lookDirection * 0.01f * mooveSpeed;
        }

        float distance = float.PositiveInfinity;
        GameObject temporaryTarget = null;

        if (targets.Count > 0) 
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] && targets[i] != gameObject)
                {
                    float targetDistance = Vector3.Distance(sight.transform.position, targets[i].transform.position);
                    if (targetDistance < distance)
                    {
                        temporaryTarget = targets[i];
                    }
                }
                else
                {
                    targets.Remove(targets[i]);
                }
            }

            if (temporaryTarget != focusedTarget)
            {
                if(focusedTarget != null)
                {
                    focusedTarget.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                temporaryTarget.GetComponent<MeshRenderer>().material.color = Color.green;
                focusedTarget = temporaryTarget;
            }
        }
        else
        {
            if (focusedTarget != null)
            {
                focusedTarget.GetComponent<MeshRenderer>().material.color = Color.red;
                focusedTarget = null;
            }
        }
    }

    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);

        onCD = false;
    }
}
