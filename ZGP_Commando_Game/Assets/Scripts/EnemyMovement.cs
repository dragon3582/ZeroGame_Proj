using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    private Transform target;
    public float moveSpeed = 1f;
    private float xScale;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        xScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {

        float move = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, move);


        if (transform.position.x < 0)
        {
            transform.localScale = new Vector3(-1 * xScale, transform.localScale.y, transform.localScale.z);
        }
        else
            transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);

    }
}
