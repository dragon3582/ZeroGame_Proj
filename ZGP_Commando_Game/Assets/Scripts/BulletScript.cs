using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public GameObject explosionParticles;

    private float speed;
    private int count = 1;
	
    void Start ()
    {
        if (this.tag == "Normal Bullet")
        {
            speed = 20f;
            this.GetComponent<Rigidbody2D>().velocity *= speed;
        }

        if (this.tag == "Spread Bullet")
        {
            speed = 75f;
            this.GetComponent<Rigidbody2D>().velocity *= speed;
        }

        if(this.tag == "Enemy Bullet")
        {
            speed = 40f;
            this.GetComponent<Rigidbody2D>().velocity *= speed;
        }

        if (this.tag == "Explosion Bullet")
        {
            speed = 55f;
            this.GetComponent<Rigidbody2D>().velocity *= speed;
            count = 1;
        }
    }

    void OnDisable()
    {
        if(this.tag == "Explosion Bullet")
        {
            GameObject temp;
            temp = Instantiate(explosionParticles, transform.position, transform.rotation) as GameObject;
            Destroy(temp, 2.0f);
            count = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(this.tag == "Normal Bullet" || this.tag == "Spread Bullet")
        {
            if(coll.collider.gameObject.tag == "Enemy Box")
            {
                Destroy(coll.gameObject);
                Destroy(this.gameObject);
            }
        }

    }

    /*
    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }
    */
}
