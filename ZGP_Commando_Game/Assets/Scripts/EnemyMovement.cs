using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public GameObject enemyBullet;
    public Transform target;

    public float moveSpeed = 2.5f;
    private float xScale;
    private float distance;
    private bool seen;
    private bool running;
    private Vector2 test = Vector2.left;
    private float timer;
    private float waitTime = 1.5f;

    void Awake()
    {
        xScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {

        float move = moveSpeed * Time.deltaTime;

        distance = Vector2.Distance(this.transform.position, target.transform.position);
        //Debug.Log(distance);
        if(distance < 20f)
        {
            seen = true;
            StartCoroutine(checkDist(move));
        }
        else if(distance > 22f)
        {
            seen = false;
            StopCoroutine(checkDist(move));
        }
            

/*
        if (transform.position.x < 0)
        {
            transform.localScale = new Vector3(-1 * xScale, transform.localScale.y, transform.localScale.z);
        }
        else
            transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
*/
    }

    void FixedUpdate()
    {
        if(seen)
        {
            fireShot();
            //Debug.Log(enemyBullet.GetComponent<Rigidbody2D>().velocity);
        }
    }

    IEnumerator checkDist(float move)
    {
        if(seen)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, move);
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
        }
    }

    void fireShot()
    {
        timer += Time.deltaTime;
        if(timer > waitTime)
        {
            GameObject tempBul;

            tempBul = Instantiate(enemyBullet, transform.position, transform.rotation) as GameObject;

            Rigidbody2D bullRig = tempBul.GetComponent<Rigidbody2D>();

            bullRig.AddForce((target.transform.position - transform.position) * Time.deltaTime, ForceMode2D.Impulse);
            //bullRig.velocity = test;
            Physics2D.IgnoreCollision(tempBul.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Destroy(tempBul, 3.0f);

            timer = 0;
        }


    }
}
