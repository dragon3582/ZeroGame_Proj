using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EnemySpawn : MonoBehaviour {

    public GameObject[] spawnPoints;
    public GameObject Enemy;
    public GameObject[] portals;

    private GameObject bossCheck;
    private List<GameObject> enemiesSpawned;
    private float spawnRate = 3f;
    private int spawner = 25;

    // Use this for initialization
    void Start()
    {
        enemiesSpawned = new List<GameObject>();
        bossCheck = GameObject.FindGameObjectWithTag("Boss");
        InvokeRepeating("spawnEnemies", 10f, spawnRate);
    }


    void spawnEnemies()
    {
        int SpawnPos = Random.Range(0, spawnPoints.Length);
        GameObject temp;
        temp = Instantiate(Enemy, spawnPoints[SpawnPos].transform.position, transform.rotation) as GameObject;
        temp.GetComponent<EnemyMovement>().bossSeen = true;
        enemiesSpawned.Add(temp);
        spawner--;
        if (spawner == 0)
        {
            CancelInvoke();
            foreach(GameObject portal in portals)
            {
                portal.GetComponent<Animation>().CrossFade("portal close");
                Destroy(portal, 4.0f);
            }
            InvokeRepeating("checkEnemyList", 2.0f, 2.0f);
        }
    }

    void checkEnemyList()
    {
        int i = 0;
        if(!bossCheck)
        {
            //enemiesSpawned.TrimExcess();
            foreach(GameObject ene in enemiesSpawned)
            {
                if (ene == null)
                {
                    i++;
                }
            }
                
        }

        if(i == 25)
        {
            SceneManager.LoadScene(6);
        }
    }

}
