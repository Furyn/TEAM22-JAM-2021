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

    private Vector3 targetPiege = new Vector3();

    private void Start()
    {
        Animator = gameObject.GetComponent<Animator>();
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        StartCoroutine(NextPos());
    }

    void Update()
    {
        if (Animator)
        {
            Animator.SetFloat("Speed", (Mathf.Abs(navAgent.velocity.x) + Mathf.Abs(navAgent.velocity.z)));
        }
        //Animator.SetFloat("Speed", );
    }

    private IEnumerator NextPos()
    {
        Vector3 destination;
        if (targetPiege == Vector3.zero)
        {
            timeUntilNextPos = Random.Range(timeMin, timeMax);
            posX = Random.Range(-10.0f, 10.0f);
            posZ = Random.Range(-10.0f, 10.0f);
            destination = new Vector3(posX, 0, posZ);
        }
        else
        {
            destination = targetPiege;
        }

        agent.SetDestination(destination);
        yield return new WaitForSeconds(timeUntilNextPos);
        StartCoroutine(NextPos());

    }

    public void SetDestination(Vector3 position)
    {
        targetPiege = position;
    }
}
