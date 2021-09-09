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

    [Header("General Settings")]
    public float durationBeforeStart = 5f;
    public float durationBlackPanel = 5f;
    public GameObject panelFade = null;
    public GameObject startButton = null;
    private Image panelFadeImage = null;
    private GameObject[] allPlayer = new GameObject[0];

    [Header("Manche Settings")]
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
    public float durationBeforeFirstEvent = 5f;

    private float timerBetweenEvent = 0f;
    private bool waitBetweenEvent = false;
    private float timerBeforeFirstEvent = 0f;
    private bool waitBeforeFirstEvent = true;
    private float timerBeforeStart = 0f;
    private bool waitBeforeStart = false;
    private float timerBlackPanel = 0f;

    private void Start()
    {
        SetUpNextManche();
        SetUpEvent();
        timerBeforeFirstEvent = durationBeforeFirstEvent;
        panelFadeImage = panelFade.GetComponent<Image>();
        timerBlackPanel = durationBlackPanel;
    }

    private void Update()
    {
        UpdateManche();

        if( waitBeforeStart )
        {
            timerBlackPanel -= Time.deltaTime;
            if (timerBlackPanel <= 0f)
            {
                timerBeforeStart -= Time.deltaTime;
                panelFadeImage.color = new Color(0, 0, 0, Mathf.InverseLerp(0, durationBeforeStart, timerBeforeStart));
                if (timerBeforeStart <= 0f)
                {
                    waitBeforeStart = false;
                    StartNextManche();
                    panelFade.SetActive(false);
                }
                
            }
        }

        if (waitBeforeFirstEvent && !waitBeforeStart)
        {
            timerBeforeFirstEvent -= Time.deltaTime;
            if (timerBeforeFirstEvent <= 0f)
            {
                waitBeforeFirstEvent = false;
                LaunchEventAlea();
            }
        }

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

        for (int i = 0; i < allPlayer.Length; i++)
        {
            float randPosX = Random.Range(startTerainPosition.position.x, endTerainPosition.position.x);
            float randPosZ = Random.Range(endTerainPosition.position.z, startTerainPosition.position.z);
            allPlayer[i].transform.position = new Vector3(randPosX, startTerainPosition.position.y, randPosZ);
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
        allPlayer = GameObject.FindGameObjectsWithTag("Player");

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
            allPlayer[i].transform.position = new Vector3(100,-100,100);
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

    public void ButtonStart()
    {

        timerText.gameObject.SetActive(true);
        mancheText.gameObject.SetActive(true);
        waitBeforeStart = true;
        timerBeforeStart = durationBeforeStart;
        panelFade.SetActive(true);
        startButton.SetActive(false);
    }

}
