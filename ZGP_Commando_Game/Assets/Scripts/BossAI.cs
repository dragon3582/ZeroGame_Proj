﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossAI : MonoBehaviour, Idamageable<float> {

    public int speed = 2;
    public GameObject bossBullet;
    public float timer;
    public float timeShot;
    public float timeShot2;
    public float timeShot3;
    public GameObject boss_healthCanvas;
    public GameObject cannon;
    public GameObject cannon2;
    public GameObject cannon3;
    public GameObject winText;
    public Transform portal;
    public Sprite defaultpic;
    public GameObject deathParticles;
    public Transform deathParticlesSpawn;

    #region private variables
    private Transform playerTarget;
    private int deathCount;
    private Vector2 direction = Vector2.right;
    private float boss_maxHealth = 4500f;
    private Image boss_fillHp;
    private float boss_currentHealth;
    public bool alive;
    private SpriteRenderer boss_alpha;
    private float waitTime;// = .2f;
    private float waitTime2;// = .5f; 
    private float waitTime3;
    private GameObject directionGO;
    private GameObject directionCannon3;
    //private Vector2 dir;
    private Animator enrage;
    private AudioSource bossGrunt;
    private bool hit;
    private Material spriteMat;
    #endregion

    void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        deathCount = 1;
        boss_currentHealth = boss_maxHealth;
        timer = Random.Range(2f, 7f);
        boss_fillHp = boss_healthCanvas.transform.GetChild(2).GetComponent<Image>();
        boss_alpha = this.gameObject.GetComponent<SpriteRenderer>();
        this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultpic;
        directionGO = cannon.transform.GetChild(0).gameObject;
        if(cannon3)
        {
            directionCannon3 = cannon3.transform.GetChild(0).gameObject;
        }
        enrage = this.gameObject.GetComponent<Animator>();
        waitTime = .2f;
        waitTime2 = .5f;
        waitTime3 = .2f;
        bossGrunt = this.gameObject.GetComponent<AudioSource>();
        hit = false;
        alive = true;
        spriteMat = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        if(alive)
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

                timer = 2.5f;
            }
        }

        if(boss_currentHealth <= 0)
        {
            alive = false;
        }
        else if(boss_currentHealth < (boss_maxHealth / 2))
        {
            enrage.SetBool("enraged", true);
            // original wait times here were .12f and .27f
            waitTime = .05f;
            waitTime2 = .12f;
            waitTime3 = .05f;
        }
        else if (boss_currentHealth > (boss_maxHealth / 2))
        {
            enrage.SetBool("enraged", false);
            // original wait times were .2f and .5f
            waitTime = .12f;
            waitTime2 = .27f;
            waitTime3 = .12f;
        }

        if (!alive)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            Camera.main.GetComponent<CameraFollow>().camMidPoint = false;
            //boss_alpha.a = .5f;
            //this.GetComponent<SpriteRenderer>().color = boss_alpha;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            //this.gameObject.GetComponent<BossAI>().enabled = false;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultpic;
            this.gameObject.GetComponent<Animator>().enabled = false;
            StartCoroutine(deadGuy());
            //InvokeRepeating("deadGuy()", .1f, .1f);
            winText.SetActive(true);
            if(portal)
            {
                portal.gameObject.SetActive(true);
            }

            if(deathCount == 1)
            {
                GameObject tempPart;
                tempPart = Instantiate(deathParticles, deathParticlesSpawn.position, deathParticlesSpawn.rotation) as GameObject;
                Destroy(tempPart, 13.0f);
                deathCount--;
            }
            

        }

        if(hit)
        {
            StartCoroutine(flashHit());
        }

    }

    void FixedUpdate()
    {
        if(alive)
        {
            fireShot();
        }
    }

    void fireShot()
    {
        timeShot += Time.deltaTime;
        timeShot2 += Time.deltaTime;
        timeShot3 += Time.deltaTime;
        if (timeShot > waitTime)
        {
            //dir = cannon.transform.position.normalized;
            //Debug.Log(dir);
            GameObject tempBullet;

            tempBullet = Instantiate(bossBullet, cannon.transform.position, directionGO.transform.rotation) as GameObject;

            tempBullet.GetComponent<BulletScript>().spawner = this.gameObject;

            Rigidbody2D bullRb = tempBullet.GetComponent<Rigidbody2D>();
            bullRb.AddForce((directionGO.transform.position - cannon.transform.position) * Time.deltaTime * 2.6f, ForceMode2D.Impulse);
            //bullRb.AddForce(Vector2.right * Time.deltaTime, ForceMode2D.Impulse);
            //bullRig.velocity = test;
            Physics2D.IgnoreCollision(tempBullet.GetComponent<Collider2D>(), GetComponent<BoxCollider2D>());
            Physics2D.IgnoreCollision(tempBullet.GetComponent<Collider2D>(), GetComponent<CircleCollider2D>());
            Physics2D.IgnoreCollision(tempBullet.GetComponent<Collider2D>(), tempBullet.GetComponent<Collider2D>());
            Destroy(tempBullet, 2.5f);

            timeShot = 0;
        }

        if (timeShot2 > waitTime2)
        {
            //dir = cannon.transform.position.normalized;
            //Debug.Log(dir);
            GameObject tempBullet2;

            tempBullet2 = Instantiate(bossBullet, cannon2.transform.position, cannon2.transform.rotation) as GameObject;

            tempBullet2.GetComponent<BulletScript>().spawner = this.gameObject;

            Rigidbody2D bullRb2 = tempBullet2.GetComponent<Rigidbody2D>();
            bullRb2.AddForce((playerTarget.transform.position - cannon2.transform.position).normalized * Time.deltaTime * 5f, ForceMode2D.Impulse);

            Physics2D.IgnoreCollision(tempBullet2.GetComponent<Collider2D>(), GetComponent<BoxCollider2D>());
            Physics2D.IgnoreCollision(tempBullet2.GetComponent<Collider2D>(), GetComponent<CircleCollider2D>());
            Physics2D.IgnoreCollision(tempBullet2.GetComponent<Collider2D>(), tempBullet2.GetComponent<Collider2D>());

            Physics2D.IgnoreLayerCollision(10, 10);
            Destroy(tempBullet2, 2.5f);

            timeShot2 = 0;
        }

        if(cannon3)
        {
            if(timeShot3 > waitTime3)
            {
                //dir = cannon.transform.position.normalized;
                //Debug.Log(dir);
                GameObject tempBullet3;

                tempBullet3 = Instantiate(bossBullet, cannon3.transform.position, directionCannon3.transform.rotation) as GameObject;

                tempBullet3.GetComponent<BulletScript>().spawner = this.gameObject;

                Rigidbody2D bullRb = tempBullet3.GetComponent<Rigidbody2D>();
                bullRb.AddForce((directionCannon3.transform.position - cannon3.transform.position) * Time.deltaTime * 2.6f, ForceMode2D.Impulse);
                //bullRb.AddForce(Vector2.right * Time.deltaTime, ForceMode2D.Impulse);
                //bullRig.velocity = test;
                Physics2D.IgnoreCollision(tempBullet3.GetComponent<Collider2D>(), GetComponent<BoxCollider2D>());
                Physics2D.IgnoreCollision(tempBullet3.GetComponent<Collider2D>(), GetComponent<CircleCollider2D>());
                Physics2D.IgnoreCollision(tempBullet3.GetComponent<Collider2D>(), tempBullet3.GetComponent<Collider2D>());
                Destroy(tempBullet3, 2.5f);

                timeShot3 = 0;
            }
        }

    }

    public void takeDamage(float damageTaken, float nothin, int nothin2)
    {
        //Debug.Log(damageTaken + " damage the enemy took.");
        //healthRegen = boss_currentHealth;
        boss_currentHealth -= damageTaken;
        //Debug.Log(boss_currentHealth);
        //StartCoroutine(updateTheUi(boss_currentHealth));
        updateTheUi(boss_currentHealth);
        bossGrunt.Play();
        hit = true;
    }

    void updateTheUi(float currentHitP)
    {
        
        float timerSec = 0f;
        float lerpin = .3f;
        currentHitP = (currentHitP / boss_maxHealth);
        /*
        while (timerSec < 1f)
        {
            timerSec += Time.deltaTime/lerpin;
            boss_fillHp.fillAmount = Mathf.Lerp(boss_fillHp.fillAmount, currentHitP, timerSec);
            yield return null;
        }
        */
        boss_fillHp.fillAmount = currentHitP;
        //yield return new WaitForEndOfFrame();
    }

    public void regenHealth(float hpRate)
    {
        boss_currentHealth += hpRate;
        //Debug.Log(hpRate + " health regenerated from " + boss_currentHealth + " to " + (boss_currentHealth - hpRate));
        //StartCoroutine(updateTheUi(boss_currentHealth));
        updateTheUi(boss_currentHealth);
    }

    IEnumerator flashHit()
    {
        boss_alpha.color = Color.red;
        yield return new WaitForSeconds(.1f);
        boss_alpha.color = Color.white;
        yield return new WaitForSeconds(.1f);
        boss_alpha.color = Color.red;
        yield return new WaitForSeconds(.1f);
        boss_alpha.color = Color.white;
        hit = false;
        if(!alive)
        {
            StopCoroutine(flashHit());
            boss_alpha.color = Color.white;
        }
    }

    IEnumerator deadGuy()
    {
        Color temp = boss_alpha.color;
        bool dissolving = true;
        //while(spriteMat.GetFloat("_DissolvePower") != 0 && boss_alpha.color.a != 0)
        if(dissolving)
        {
            spriteMat.SetFloat("_DissolvePower", Mathf.Clamp((spriteMat.GetFloat("_DissolvePower") - (.1f * Time.deltaTime)), 0f, .99f));
            temp.a -= (.08f * Time.deltaTime);
            boss_alpha.color = temp;
            yield return new WaitForSeconds(.005f);
            if(spriteMat.GetFloat("_DissolvePower") == 0)
            {
                dissolving = false;
            }
        }

        if(!dissolving)
        {
            //yield return new WaitForSeconds(3.5f);
            Destroy(this.gameObject);
        }
        
    }

    void OnDisable()
    {
        //CancelInvoke();
        StopAllCoroutines();
    }

}
