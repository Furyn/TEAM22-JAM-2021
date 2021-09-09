using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim2: MonoBehaviour
{
    #region Variables

    //Sight varibles
    public List<GameObject> targets;
    private GameObject focusedTarget;

    private Vector2 lookInput = Vector2.zero;

    [SerializeField] private float mooveSpeed;
    public GameObject sight;

    public Vector3 spawnPoint;

    public Color sightColor;

    //GameManager variables
    [HideInInspector] public int pNb = 1;
    [SerializeField] private int playerKillScore;

    private GameManager gameManager;

    [HideInInspector] public bool imDead = false;

    //Shot variables
    private bool shotOnCD = false;
    [SerializeField] private float cooldown;


    //NPC variables
    private GameObject NPC;
    private bool NPCPlayerFollow = false;
    [SerializeField] private float followTime;
    private float currentFollowTime;
    [SerializeField] private float NPCHitDistance;
    [SerializeField] private float nPCAttackCooldownTime;
    private bool nPCAttackOnCD;
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
        if(!shotOnCD && focusedTarget && !imDead)
        {
            Debug.Log("boom");

            if(focusedTarget.tag == "Player")
            {
                int score = (int)gameManager.GetType().GetField("p" + pNb + "Score").GetValue(gameManager);
                gameManager.GetType().GetField("p" + pNb + "Score").SetValue(gameManager, score + playerKillScore);
                score = (int)gameManager.GetType().GetField("p" + pNb + "Score").GetValue(gameManager);
                Debug.Log("Player " + pNb + " score: " + score);

                focusedTarget.GetComponent<PlayerController>().DIE();
            }
            else if(focusedTarget.tag == "NPC")
            {
                NPCPlayerFollow = true;
                NPC = focusedTarget;
            }

            sight.transform.position = spawnPoint;

            shotOnCD = true;
            StartCoroutine(ShotCooldown(cooldown));
        }
        else if (shotOnCD)
        {
            Debug.Log("onCooldown");
        }
    }

    void Update()
    {
        #region Sight movement
        Vector3 lookDirection = new Vector3(lookInput.x, 0, lookInput.y);

        if (!shotOnCD && !imDead)
        {
            sight.transform.position += lookDirection * 0.01f * mooveSpeed;
        }
        #endregion

        #region Targets detection
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
                if(focusedTarget != null && (focusedTarget.tag == "NPC" && focusedTarget.GetComponent<AIBehaviour>().sightsNb == 1) | (focusedTarget.tag == "player" && focusedTarget.GetComponent<PlayerController>().sightsNb == 1))
                {
                    if (focusedTarget.tag == "NPC")
                    {
                        focusedTarget.GetComponent<AIBehaviour>().sightsNb += -1;
                    }
                    else if (focusedTarget.tag == "Player")
                    {
                        focusedTarget.GetComponent<PlayerController>().sightsNb += -1;
                    }

                    focusedTarget.GetComponent<MeshRenderer>().material.color = Color.red;
                }

                if (temporaryTarget && temporaryTarget.tag == "NPC")
                {
                    temporaryTarget.GetComponent<AIBehaviour>().sightsNb += 1;
                }
                else if (temporaryTarget && temporaryTarget.tag == "Player")
                {
                    temporaryTarget.GetComponent<PlayerController>().sightsNb += 1;
                }

                temporaryTarget.GetComponent<MeshRenderer>().material.color = Color.green;
                focusedTarget = temporaryTarget;
            }
        }
        else
        {
            if (focusedTarget != null && (focusedTarget.tag == "NPC" && focusedTarget.GetComponent<AIBehaviour>().sightsNb == 1) | (focusedTarget.tag == "player" && focusedTarget.GetComponent<PlayerController>().sightsNb == 1))
            {
                if (focusedTarget.tag == "NPC")
                {
                    focusedTarget.GetComponent<AIBehaviour>().sightsNb += -1;
                }
                else if (focusedTarget.tag == "Player")
                {
                    focusedTarget.GetComponent<PlayerController>().sightsNb += -1;
                }

                focusedTarget.GetComponent<MeshRenderer>().material.color = Color.red;
                focusedTarget = null;
            }
        }
        #endregion

        #region NPC player Follow
        if(NPCPlayerFollow && NPC && !imDead)
        {
            if(currentFollowTime < followTime)
            {
                NPC.GetComponent<AIBehaviour>().SetDestination(transform.position);
                currentFollowTime += Time.deltaTime;
                if(Vector3.Distance(transform.position, NPC.transform.position) < NPCHitDistance)
                {
                    if (!nPCAttackOnCD)
                    {
                        //HitPlayer
                        nPCAttackOnCD = true;
                        NPCAttackCooldown(nPCAttackCooldownTime);
                    }
                }
            }
            else
            {
                NPCPlayerFollow = false;
                NPC.GetComponent<AIBehaviour>().SetDestination(Vector3.zero);
                currentFollowTime = 0;
                NPC = null;
            }

        }
        else if (NPCPlayerFollow && NPC && imDead)
        {
            NPCPlayerFollow = false;
            focusedTarget.GetComponent<AIBehaviour>().SetDestination(Vector3.zero);
            currentFollowTime = 0;
            NPC = null;
        }
        #endregion
    }

    IEnumerator ShotCooldown(float time)
    {
        yield return new WaitForSeconds(time);

        shotOnCD = false;
    }

    IEnumerator NPCAttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);

        nPCAttackOnCD = false;
    }
}
