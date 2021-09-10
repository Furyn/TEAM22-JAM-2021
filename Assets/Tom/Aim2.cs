using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Aim2: MonoBehaviour
{
    #region Variables

    [Header("Sight")]
    [HideInInspector] public List<GameObject> targets;
    private GameObject focusedTarget;

    private Vector2 lookInput = Vector2.zero;

    [SerializeField] private float sightMooveSpeed;
    [HideInInspector] public GameObject sight;

    public Vector3 spawnPoint;
    [SerializeField] private bool sightPositionRestOnShot = true;

    public Color sightColor;

    [HideInInspector]public bool shotOnCD = false;
    [SerializeField] private float shotCooldown;

    [Header("GameManager")]
    [HideInInspector] public int pNb = 1;
    [SerializeField] private int playerKillScore;

    private GameManager gameManager;

    [HideInInspector] public bool imDead = false;

    [SerializeField] private GameObject Marker;
    private GameObject targetMarker;

    [Header("NPC")]
    private GameObject NPC;
    private bool NPCPlayerFollow = false;
    [SerializeField] private float followTime;
    private float currentFollowTime;
    [SerializeField] private float NPCHitDistance;
    [SerializeField] private float nPCAttackCooldownTime;
    [SerializeField] private GameObject blood;
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
                focusedTarget.GetComponent<PlayerController>().marker.SetActive(false);
            }
            else if(focusedTarget.tag == "NPC")
            {
                NPCPlayerFollow = true;
                NPC = focusedTarget;

                focusedTarget.GetComponent<AIBehaviour>().marker.SetActive(false);
            }

            if (sightPositionRestOnShot)
            {
                sight.transform.position = spawnPoint;
            }

            shotOnCD = true;
            StartCoroutine(ShotCooldown(shotCooldown));
        }
        else if (shotOnCD)
        {
            Debug.Log("onCooldown");
        }
    }

    void Update()
    {

        if (Time.deltaTime == 0)
        {
            return;
        }
        #region Sight movement
        Vector3 lookDirection = new Vector3(lookInput.x, 0, lookInput.y);

        if (!shotOnCD && !imDead)
        {
            sight.transform.position += lookDirection * Time.deltaTime * sightMooveSpeed;
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

            if (temporaryTarget != focusedTarget && !shotOnCD)
            {
                if(focusedTarget != null)
                {
                    if (focusedTarget.tag == "NPC")
                    {
                        focusedTarget.GetComponent<AIBehaviour>().sightsNb += -1;
                    }
                    else if (focusedTarget.tag == "Player")
                    {
                        focusedTarget.GetComponent<PlayerController>().sightsNb += -1;
                    }

                    if(focusedTarget.tag == "NPC" && focusedTarget.GetComponent<AIBehaviour>().sightsNb == 0)
                    {
                        focusedTarget.GetComponent<AIBehaviour>().marker.SetActive(false);
                    }
                    else if (focusedTarget.tag == "Player" && focusedTarget.GetComponent<PlayerController>().sightsNb == 0)
                    {
                        focusedTarget.GetComponent<PlayerController>().marker.SetActive(false);
                    }
                }

                if (temporaryTarget && temporaryTarget.tag == "NPC")
                {
                    AIBehaviour controller = temporaryTarget.GetComponent<AIBehaviour>();
                    controller.sightsNb += 1;
                    controller.marker.SetActive(true);
                    focusedTarget = temporaryTarget;
                }
                else if (temporaryTarget && temporaryTarget.tag == "Player")
                {
                    PlayerController controller = temporaryTarget.GetComponent<PlayerController>();
                    controller.sightsNb += 1;
                    controller.marker.SetActive(true);
                    focusedTarget = temporaryTarget;
                }
            }
        }
        else
        {
            if (focusedTarget != null)
            {
                if (focusedTarget.tag == "NPC")
                {
                    focusedTarget.GetComponent<AIBehaviour>().sightsNb += -1;
                }
                else if (focusedTarget.tag == "Player")
                {
                    focusedTarget.GetComponent<PlayerController>().sightsNb += -1;
                }

                if (focusedTarget.tag == "NPC" && focusedTarget.GetComponent<AIBehaviour>().sightsNb == 0)
                {
                    focusedTarget.GetComponent<AIBehaviour>().marker.SetActive(false);
                }
                else if (focusedTarget.tag == "Player" && focusedTarget.GetComponent<PlayerController>().sightsNb == 0)
                {
                    focusedTarget.GetComponent<PlayerController>().marker.SetActive(false);
                }

                focusedTarget = null;
            }
        }
        #endregion

        #region NPC player Follow
        if(NPCPlayerFollow && NPC && !imDead)
        {
            if(currentFollowTime < followTime)
            {
                NPC.GetComponent<AIBehaviour>().SetDestination(new Vector3(transform.position.x, 0, transform.position.z));
                currentFollowTime += Time.deltaTime; 
                if(Vector3.Distance(transform.position, NPC.transform.position) < NPCHitDistance)
                {
                    Debug.Log("Punch"!);
                    blood.GetComponent<ParticleSystem>().Play(true);
                    NPCPlayerFollow = false;
                    NPC.GetComponent<AIBehaviour>().SetDestination(Vector3.zero);
                    currentFollowTime = 0;
                    NPC = null;
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

    private IEnumerator ShotCooldown(float time)
    {
        yield return new WaitForSeconds(time);

        shotOnCD = false;
        StopCoroutine(ShotCooldown(1));
    }
}
