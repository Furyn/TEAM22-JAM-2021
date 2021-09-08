using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int current_manche = 0;
    public int manche = 3;
    public Transform startTerainPosition = null;
    public Transform endTerainPosition = null;

    [Header("Trap Settings")]
    public GameObject[] AllTrap = null;
    public int nbMinTrap = 1;
    public int nbMaxTrap = 10;
    public Transform posParentTrap = null;

    

    private void Start()
    {
        SetUpNextManche();
        StartNextManche();
    }

    public void SetUpNextManche()
    {
        //Ajout des joueurs / NPC
    }


    public void StartNextManche()
    {
        current_manche++;
        int nbrandTrap = Random.Range(nbMinTrap, nbMaxTrap);
        for (int i = 0; i < nbrandTrap; i++)
        {
            GameObject randTrap = AllTrap[Random.Range(0, AllTrap.Length)];
            float randPosX = Random.Range(startTerainPosition.position.x, endTerainPosition.position.x);
            float randPosZ = Random.Range(endTerainPosition.position.z, startTerainPosition.position.z);
            Instantiate(randTrap, new Vector3(randPosX, randTrap.transform.position.y, randPosZ), Quaternion.identity, posParentTrap);
        }

    }

}
