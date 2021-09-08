using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("Manche settings")]
    public int current_manche = 0;
    public int manche = 3;
    public float durationManche = 30f;
    public float durationBetweenManche = 5f;
    public Transform startTerainPosition = null;
    public Transform endTerainPosition = null;
    public Text timerText = null;

    private float timerManche = 0f;
    private bool mancheStarted = false;
    private float timerBetweenManche = 0f;
    private bool waitBetweenManche = false;

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

    private void Update()
    {
        UpdateManche();

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

    public void SetUpNextManche()
    {
        timerManche = durationManche;

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
        mancheStarted = true;
        timerManche = durationManche;
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

    public void StopCurrentManche()
    {
        mancheStarted = false;
        timerManche = 0f;
        timerBetweenManche = durationBetweenManche;
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

    private void UpdateManche()
    {

        timerText.text = ((int)timerManche).ToString();
        if (mancheStarted)
        {
            timerManche -= Time.deltaTime;

            if (timerManche <= 0f)
            {
                StopCurrentManche();
            }
        }

        if (waitBetweenManche)
        {

        }
    }

}
