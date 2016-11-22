using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public GameObject explosionParticles;
    public GameObject spawner;
    public GameObject particles;

    Idamageable<float> damage;
    Idamageable<int> playerHit;
    private float damageNorm;// = 25.0f;   original values i used before i randomized them a little
    private float damageSpread;// = 30.0f;
    private int enemyDamage = 1;
    private float speed;
    //private int count = 1;
    private int counter = 1;
    private float healthRegen = 50f;
    private AudioSource soundEffect;

    void Start ()
    {
        soundEffect = this.gameObject.GetComponent<AudioSource>();
        if (this.tag == "Normal Bullet")
        {
            // original speed was 20f
            speed = 40f;
            this.GetComponent<Rigidbody2D>().velocity *= speed;
            damageNorm = Mathf.Ceil(Random.Range(23f, 26f));
        }

        if (this.tag == "Spread Bullet")
        {
            // original speed was 75f
            speed = 100f;
            this.GetComponent<Rigidbody2D>().velocity *= speed;
            damageSpread = Mathf.Ceil(Random.Range(28f, 33f));
        }

        if(this.tag == "Enemy Bullet")
        {
            // original speed was 40f before being normalized in enemy movement scrip
            speed = 750f;
            this.GetComponent<Rigidbody2D>().velocity *= speed;
        }

        if (this.tag == "Boss bullet")
        {
            // original speed was 450f
            speed = 1050f;
            this.GetComponent<Rigidbody2D>().velocity *= speed;
        }

        if (this.tag == "Explosion Bullet")
        {
            speed = 55f;
            this.GetComponent<Rigidbody2D>().velocity *= speed;
            //count = 1;
        }
    }

    void OnDisable()
    {
        if(this.tag == "Explosion Bullet")
        {
            GameObject temp;
            temp = Instantiate(explosionParticles, transform.position, transform.rotation) as GameObject;
            Destroy(temp, 2.0f);
            //count = 0;
        }
        else if(this.tag == "Normal Bullet" || this.tag == "Spread Bullet")
        {
            GameObject temp2;
            temp2 = Instantiate(particles, transform.position, transform.rotation) as GameObject;
            Destroy(temp2, 5.0f);
        }
        else if (this.tag == "Enemy Bullet")
        {
            GameObject temp3;
            temp3 = Instantiate(particles, transform.position, transform.rotation) as GameObject;
            Destroy(temp3, 5.0f);
        }
    }

    //initialize the interface variables to call the damage function and apply the damage each bullet has
    void OnCollisionEnter2D(Collision2D coll)
    {
        damage = (Idamageable<float>)coll.gameObject.GetComponent(typeof(Idamageable<float>));
        playerHit = (Idamageable<int>)coll.gameObject.GetComponent(typeof(Idamageable<int>));
        if (this.tag == "Normal Bullet" || this.tag == "Spread Bullet")
        {
            if(coll.collider.gameObject.tag == "Enemy Box")
            {
                if(this.tag == "Normal Bullet")
                {
                    damage.takeDamage(damageNorm);
                }
                else if(this.tag == "Spread Bullet")
                {
                    damage.takeDamage(damageSpread);
                }
                //Destroy(coll.gameObject);
                Destroy(this.gameObject);
            }
        }
        else if(coll.collider.gameObject.tag == "Player" && counter != 0)
        {
            if(this.tag == "Enemy Bullet" || this.tag == "Boss bullet")
            {
                playerHit.takeDamage(enemyDamage);
                if(this.tag == "Boss bullet")
                {
                    spawner.GetComponent<BossAI>().regenHealth(healthRegen);
                    GameObject temp2;
                    temp2 = Instantiate(particles, transform.position, transform.rotation) as GameObject;
                    Destroy(temp2, 3.0f);
                }
            }
            counter--;
            //Destroy(this.gameObject);
        }

    }

    /*
    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }
    */
}
