using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AIBehaviour : MonoBehaviour
{
    private Animator Animator;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float timeMin = 0.5f;
    [SerializeField] private float timeMax = 5.0f;

    private NavMeshAgent navAgent;

    private float timeUntilNextPos;
    private float posX;
    private float posZ;
    public float maxPosX;
    public float minPosX;
    public float maxPosY;
    public float minPosY;

    private Vector3 newDestination = new Vector3();

    [HideInInspector] public int sightsNb;
    [HideInInspector] public GameObject marker;
    [SerializeField] private GameObject markerPrefab;

    private void Start()
    {
        Animator = gameObject.GetComponent<Animator>();
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        marker = GameObject.Instantiate(markerPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation, transform);
        marker.SetActive(false);
        StartCoroutine(NextPos());
    }

    void Update()
    {
        if (Animator)
        {
            Animator.SetFloat("Speed", (Mathf.Abs(navAgent.velocity.x) + Mathf.Abs(navAgent.velocity.z)));
        }
        if (timeUntilNextPos > 0f && newDestination != Vector3.zero)
        {
            StopCoroutine(NextPos());
            StartCoroutine(NextPos());
        }
    }

    private IEnumerator NextPos()
    {
        Vector3 destination;
        if (newDestination == Vector3.zero)
        {
            timeUntilNextPos = Random.Range(timeMin, timeMax);
            posX = Random.Range(minPosX, maxPosX);
            posZ = Random.Range(minPosY, maxPosY);
            destination = new Vector3(posX, 0, posZ);
        }
        else
        {
            destination = newDestination;
            timeUntilNextPos = 0f;
        }

        agent.SetDestination(destination);
        yield return new WaitForSeconds(timeUntilNextPos);
        StartCoroutine(NextPos());

    }

    public void SetDestination(Vector3 position)
    {
        newDestination = position;
    }
}
