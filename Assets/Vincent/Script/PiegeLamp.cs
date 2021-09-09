using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiegeLamp : MonoBehaviour
{

    private List<GameObject> allPlayer = new List<GameObject>();
    [SerializeField] private GameObject marker = null;

    Dictionary<GameObject, GameObject> allPlayer_marker = new Dictionary<GameObject, GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            GameObject marker_ins = GameObject.Instantiate(marker, new Vector3(other.transform.position.x, other.transform.position.y + 1, other.transform.position.z), other.transform.rotation, other.transform);
            allPlayer.Add(other.gameObject);
            allPlayer_marker.Add(other.gameObject, marker_ins);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(allPlayer_marker[other.gameObject]);

            allPlayer_marker.Remove(other.gameObject);
            allPlayer.Remove(other.gameObject);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject player in allPlayer)
        {
            Destroy(allPlayer_marker[player]);

            allPlayer_marker.Remove(player);
        }
    }

}
