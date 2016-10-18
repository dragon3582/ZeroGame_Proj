using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour {

    public int speed = 2;

    public GameObject Boss;

    public GameObject enemy_bullet;

    private Vector2 direction = Vector2.right;

    public float timer;

    void Start()
    {
        timer = Random.Range(5, 10);
    }

    void Update()
    {

        transform.Translate(direction * speed * Time.deltaTime);

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            int rand_num = Random.Range(0, 2);
            int num_chosen = rand_num;

            if (num_chosen <= 2)
            {

                direction = -direction;
                
            }

            timer = Random.Range(3, 7);
        }
    }

}
