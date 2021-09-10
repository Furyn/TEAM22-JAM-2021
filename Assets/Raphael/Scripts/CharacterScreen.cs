using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterScreen : MonoBehaviour
{
    public GameObject characterScreen;
    public GameObject playButton;

    [SerializeField] private GameObject mesh1;
    [SerializeField] private GameObject mesh2;
    [SerializeField] private GameObject mesh3;
    [SerializeField] private GameObject mesh4;

    [SerializeField] private GameObject startButton;

    private GameObject playerManager;
    public int playerCount;
    public Text instructions;
    public GameManager gm = null;

    void Start()
    {
        characterScreen.SetActive(true);
        playerManager = GameObject.Find("PlayerManager");
        playButton.SetActive(false);
    }


    void Update()
    {
        if (characterScreen.activeSelf)
        {
            Time.timeScale = 0f;
        }

        //if (playerCount >= 2)
        //{
        //    playButton.SetActive(true);
        //}

    }

    public void NewPlayer()
    {
        playerCount = playerManager.GetComponent<PlayerCount>().playerCount;

        if (playerCount == 1)
        {
            mesh1.SetActive(true);
            instructions.text = "Player 2 press A";
        }

        else if (playerCount == 2)
        {
            mesh2.SetActive(true);
            playButton.SetActive(true);
            instructions.text = "Player 3 press A";
        }

        else if (playerCount == 3)
        {
            mesh3.SetActive(true);
            instructions.text = "Player 4 press A";
        }

        else if (playerCount == 4)
        {
            mesh4.SetActive(true);
            instructions.text = "The room is full !";
        }
    }

    public void OnPlay()
    {
        gm.ButtonStart();
        startButton.SetActive(true);
        characterScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
