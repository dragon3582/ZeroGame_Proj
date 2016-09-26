using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    private float speed;
	
    void Start ()
    {
        if (this.tag == "Normal Bullet")
        {
            speed = 20f;
            this.GetComponent<Rigidbody2D>().velocity *= speed;
            //Debug.Log(this.GetComponent<Rigidbody2D>().velocity);
        }

        if (this.tag == "Spread Bullet")
        {
            speed = 75f;
            this.GetComponent<Rigidbody2D>().velocity *= speed; 
            //Debug.Log(this.GetComponent<Rigidbody2D>().velocity);
        }
    }

}
