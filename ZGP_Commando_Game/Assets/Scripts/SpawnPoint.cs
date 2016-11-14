using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

    private Transform playerSpawn;

	void Awake ()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("Player").transform;
	}

    void Start ()
    {
        if(playerSpawn.gameObject)
        {
            playerSpawn.position = this.gameObject.transform.position;
        }
    }
}
