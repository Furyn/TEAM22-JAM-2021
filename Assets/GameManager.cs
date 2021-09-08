using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int current_manche = 0;
    public int manche = 3;
    public Transform startTerainPosition = null;
    public Transform endTerainPosition = null;

    [Header("Trap Settings")]
    public GameObject[] allTrap = null;
    public int nbMinTrap = 1;
    public int nbMaxTrap = 10;
    public Transform posParentTrap = null;

    [Header("NPC Settings")]
    public GameObject[] allNpcs = null;
    public int nbNpc = 10;
    public Transform posParentNPC = null;

    [Header("Event Settigns")]
    public Event[] allEvent = null;
    public float durationBetweenEvent = 5f;
    private float timerBetweenEvent = 0f;
    private bool waitBetweenEvent = false;

    private void Start()
    {
        SetUpNextManche();
        StartNextManche();
        LaunchEventAlea();
    }

    public void SetUpNextManche()
    {
        for (int i = 0; i < nbNpc; i++)
        {
            GameObject randNpc = allNpcs[Random.Range(0, allNpcs.Length)];
            float randPosX = Random.Range(startTerainPosition.position.x, endTerainPosition.position.x);
            float randPosZ = Random.Range(endTerainPosition.position.z, startTerainPosition.position.z);
            Instantiate(randNpc, new Vector3(randPosX, randNpc.transform.position.y, randPosZ), Quaternion.identity, posParentNPC);
        }
    }

    public void StartNextManche()
    {
        current_manche++;
        int nbrandTrap = Random.Range(nbMinTrap, nbMaxTrap);
        for (int i = 0; i < nbrandTrap; i++)
        {
            GameObject randTrap = allTrap[Random.Range(0, allTrap.Length)];
            float randPosX = Random.Range(startTerainPosition.position.x, endTerainPosition.position.x);
            float randPosZ = Random.Range(endTerainPosition.position.z, startTerainPosition.position.z);
            Instantiate(randTrap, new Vector3(randPosX, randTrap.transform.position.y, randPosZ), Quaternion.identity, posParentTrap);
        }

    }

    private void Update()
    {
        if (waitBetweenEvent)
        {
            timerBetweenEvent -= Time.deltaTime;

            if (timerBetweenEvent <= 0f && !EventIsActive())
            {
                LaunchEventAlea();
                waitBetweenEvent = false;
            }
        }

        if (!waitBetweenEvent && !EventIsActive())
        {
            waitBetweenEvent = true;
            timerBetweenEvent = durationBetweenEvent;
        }
        
    }


    public void LaunchEventAlea()
    {
        Event randEvent = allEvent[Random.Range(0, allEvent.Length)];
        randEvent.Apply();
    }

    private bool EventIsActive()
    {
        bool oneIsActive = false;
        foreach (Event element in allEvent)
        {
            if (element.eventLaunch)
            {
                oneIsActive = true;
            }
        }
        return oneIsActive;
    }

}
