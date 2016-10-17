using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour {

    static bool go_left = false;
    public int speed = 5;

    public GameObject Boss;

    public GameObject enemy_bullet;

    private int timer = 10;

    void Update()
    {
        if (go_left)
        {
            
            transform.Translate(-Vector2.right * speed * Time.deltaTime);

        }
        else
        {
            
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }

        /*Firing code for the boss. It has a timer.*/
        //timer -= Time.deltaTime;

        //if (timer < 2)
        //{
            //int rand_num = Random.Range(0, 20);
            //int num_chosen = rand_num;

            //if (num_chosen == 2)
            //{
                //FIXME_VAR_TYPE fire_bullet = Instantiate(enemy_bullet, transform.position, Quaternion.identity);
                //fire_bullet.GetComponent.< Rigidbody > ().AddForce(-Vector3.up * 10);
            //}

            //timer = 10;
        //}
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "LeftWall")
        {
            go_left = false;
        }
        else if (col.gameObject.tag == "RightWall")
        {
            go_left = true;
        }
    }


}
