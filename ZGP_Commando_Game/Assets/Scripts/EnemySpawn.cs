using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {

    public GameObject[] spawnPoints;
    public GameObject Enemy;
    public float spawnRate = 1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        InvokeRepeating("spawnEnemies", spawnRate, spawnRate);

    }

    void spawnEnemies()
    {
        int SpawnPos = Random.Range(0, spawnPoints.Length);
        Instantiate(Enemy, spawnPoints[SpawnPos].transform.position, transform.rotation);
        CancelInvoke();
    }
}
