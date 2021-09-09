using System.Collections.Generic;
using UnityEngine;

public class EventIA : Event
{
    [SerializeField] private GameObject[] allPrefabPiege = new GameObject[0];
    [SerializeField] private int minPrefab = 2;
    [SerializeField] private int maxPrefab = 10;

    private bool wasSetup = false;
    private List<GameObject> allPiegeInstantiate = new List<GameObject>();

    public override void UpdateEvent()
    {
        if (eventLaunch && !wasSetup)
        {
            wasSetup = true;
            int nbPrefab = Random.Range(minPrefab, maxPrefab);

            for (int i = 0; i < nbPrefab; i++)
            {
                GameObject randPiege = allPrefabPiege[Random.Range(0, allPrefabPiege.Length)];
                float randPosX = Random.Range(startTerainPosition.position.x, endTerainPosition.position.x);
                float randPosZ = Random.Range(endTerainPosition.position.z, startTerainPosition.position.z);
                allPiegeInstantiate.Add(Instantiate(randPiege, new Vector3(randPosX, randPiege.transform.position.y, randPosZ), Quaternion.identity, gameObject.transform));
            }

        }
        else if (!eventLaunch)
        {
            wasSetup = false;
        }

        base.UpdateEvent();
    }

    public override void Stop()
    {
        GameObject[] allPiageArray = allPiegeInstantiate.ToArray();
        if (allPiageArray.Length > 0)
        {
            for (int i = 0; i < allPiageArray.Length; i++)
            {
                Destroy(allPiageArray[i]);
            }
        }
        allPiegeInstantiate.Clear();
        
        base.Stop();
    }

}
