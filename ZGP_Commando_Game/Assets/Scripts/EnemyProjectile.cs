using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {
    public Vector3 bulletOffset = new Vector3(0, 0.5f, 0);

    public GameObject enemyBullet;

    public float fireDelay = 3.0f;
    float cooldownTimer = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            cooldownTimer = fireDelay;

            Vector3 offset = transform.rotation * bulletOffset;

            Instantiate(enemyBullet, transform.position + offset, transform.rotation);
        }
	}
}
