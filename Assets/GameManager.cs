using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Players scores
    [HideInInspector] public int p1Score;
    [HideInInspector] public int p2Score;
    [HideInInspector] public int p3Score;
    [HideInInspector] public int p4Score;

    [Header("Manche settings")]
    public int current_manche = 0;
    public int manche = 3;
    public float durationManche = 30f;
    public float durationBetweenManche = 5f;
    public Transform startTerainPosition = null;
    public Transform endTerainPosition = null;
    public Text timerText = null;
    public Text mancheText = null;
    public string messageManche = "Manche : ";
    public PlayerInputManager playerInputManager = null;

    private int nbPlayer = 0;
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
        SetUpEvent();
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
        waitBetweenManche = false;
    
        for (int i = 0; i < nbNpc; i++)
        {
            GameObject randNpc = allNpcs[Random.Range(0, allNpcs.Length)];
            float randPosX = Random.Range(startTerainPosition.position.x, endTerainPosition.position.x);
            float randPosZ = Random.Range(endTerainPosition.position.z, startTerainPosition.position.z);
            Instantiate(randNpc, new Vector3(randPosX, randNpc.transform.position.y, randPosZ), Quaternion.identity, posParentNPC);
        }

        Debug.Log(nbPlayer);
        for (int i = 0; i < nbPlayer; i++)
        {
            float randPosX = Random.Range(startTerainPosition.position.x, endTerainPosition.position.x);
            float randPosZ = Random.Range(endTerainPosition.position.z, startTerainPosition.position.z);
            Instantiate(playerInputManager.playerPrefab, new Vector3(randPosX, playerInputManager.playerPrefab.transform.position.y, randPosZ), Quaternion.identity, posParentNPC);
            
        }

    }

    public void StartNextManche()
    {
        playerInputManager.EnableJoining();
        mancheStarted = true;
        timerManche = durationManche;
        current_manche++;
        mancheText.text = messageManche + current_manche;
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
        waitBetweenManche = true;
        playerInputManager.DisableJoining();
        GameObject[] allPlayer = GameObject.FindGameObjectsWithTag("Player");
        nbPlayer = allPlayer.Length;

        for (int i = 0; i < posParentNPC.childCount; i++)
        {
            Destroy(posParentNPC.GetChild(i).gameObject);
        }
    
        for (int i = 0; i < posParentTrap.childCount; i++)
        {
            Destroy(posParentTrap.GetChild(i).gameObject);
        }

        for (int i = 0; i < allPlayer.Length; i++)
        {
            Destroy(allPlayer[i]);
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
            timerBetweenManche -= Time.deltaTime;
    
            if (timerBetweenManche <= 0f && current_manche < manche)
            {
                SetUpNextManche();
                StartNextManche();
            }
        }
    }

    private void SetUpEvent()
    {
        foreach (Event item in allEvent)
        {
            item.SetUpPostion(startTerainPosition, endTerainPosition);
        }
    }

}
