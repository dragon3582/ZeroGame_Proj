using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossAI : MonoBehaviour, Idamageable<float> {

    public int speed = 2;
    public GameObject bossBullet;
    public float timer;
    public float timeShot;
    public float timeShot2;
    public GameObject boss_healthCanvas;
    public GameObject cannon;
    public GameObject cannon2;
    public GameObject winText;
    public Transform portal;
    public Sprite defaultpic;

    #region private variables
    private Transform playerTarget;
    private Vector2 direction = Vector2.right;
    private float boss_maxHealth = 4500f;
    private Image boss_fillHp;
    private float boss_currentHealth;
    public bool alive;
    private Color boss_alpha;
    private float waitTime;// = .2f;
    private float waitTime2;// = .5f; 
    private GameObject directionGO;
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
        boss_currentHealth = boss_maxHealth;
        timer = Random.Range(2f, 7f);
        boss_fillHp = boss_healthCanvas.transform.GetChild(2).GetComponent<Image>();
        boss_alpha = this.gameObject.GetComponent<SpriteRenderer>().color;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultpic;
        directionGO = cannon.transform.GetChild(0).gameObject;
        enrage = this.gameObject.GetComponent<Animator>();
        waitTime = .2f;
        waitTime2 = .5f;
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
            waitTime2 = .07f;
        }
        else if (boss_currentHealth > (boss_maxHealth / 2))
        {
            enrage.SetBool("enraged", false);
            // original wait times were .2f and .5f
            waitTime = .12f;
            waitTime2 = .27f;
        }

        if (!alive)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            boss_alpha.a = .5f;
            this.GetComponent<SpriteRenderer>().color = boss_alpha;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            this.gameObject.GetComponent<BossAI>().enabled = false;
            this.gameObject.GetComponent<Animator>().enabled = false;
            StartCoroutine(deadGuy());
            //InvokeRepeating("deadGuy()", .1f, .1f);
            winText.SetActive(true);
            portal.gameObject.SetActive(true);

        }

        if(hit)
        {
            StartCoroutine(flashHit());
        }

    }

    void FixedUpdate()
    {
        fireShot();
    }

    void fireShot()
    {
        timeShot += Time.deltaTime;
        timeShot2 += Time.deltaTime;
        if (timeShot > waitTime)
        {
            //dir = cannon.transform.position.normalized;
            //Debug.Log(dir);
            GameObject tempBullet;

            tempBullet = Instantiate(bossBullet, cannon.transform.position, directionGO.transform.rotation) as GameObject;

            tempBullet.GetComponent<BulletScript>().spawner = this.gameObject;

            Rigidbody2D bullRb = tempBullet.GetComponent<Rigidbody2D>();
            bullRb.AddForce((directionGO.transform.position - cannon.transform.position) * Time.deltaTime, ForceMode2D.Impulse);
            //bullRb.AddForce(Vector2.right * Time.deltaTime, ForceMode2D.Impulse);
            //bullRig.velocity = test;
            Physics2D.IgnoreCollision(tempBullet.GetComponent<Collider2D>(), GetComponent<BoxCollider2D>());
            Physics2D.IgnoreCollision(tempBullet.GetComponent<Collider2D>(), GetComponent<CircleCollider2D>());
            Physics2D.IgnoreCollision(tempBullet.GetComponent<Collider2D>(), tempBullet.GetComponent<Collider2D>());
            Destroy(tempBullet, 3.0f);

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
            bullRb2.AddForce((playerTarget.transform.position - cannon2.transform.position).normalized * 2f * Time.deltaTime, ForceMode2D.Impulse);

            Physics2D.IgnoreCollision(tempBullet2.GetComponent<Collider2D>(), GetComponent<BoxCollider2D>());
            Physics2D.IgnoreCollision(tempBullet2.GetComponent<Collider2D>(), GetComponent<CircleCollider2D>());
            Physics2D.IgnoreCollision(tempBullet2.GetComponent<Collider2D>(), tempBullet2.GetComponent<Collider2D>());

            Physics2D.IgnoreLayerCollision(10, 10);
            Destroy(tempBullet2, 4.0f);

            timeShot2 = 0;
        }

    }

    public void takeDamage(float damageTaken)
    {
        //Debug.Log(damageTaken + " damage the enemy took.");
        //healthRegen = boss_currentHealth;
        boss_currentHealth -= damageTaken;
        //Debug.Log(boss_currentHealth);
        //StartCoroutine(updateTheUi(previousHealth, currentHealth));
        updateTheUi(boss_currentHealth);
        bossGrunt.Play();
        hit = true;
    }

    void updateTheUi(float currentHP)
    {
        //float timer = 0f;
        currentHP = (currentHP / boss_maxHealth);
        //hPregen = (hPregen / boss_maxHealth);
        /*
        if (timer < lerpSpeed)
        {
            fillHp.fillAmount = Mathf.Lerp(fillHp.fillAmount, currentHP, (lerpSpeed/timer));
            timer += Time.deltaTime;
        }
        */
        boss_fillHp.fillAmount = currentHP;
        //yield return new WaitForEndOfFrame();
    }

    public void regenHealth(float hpRate)
    {
        boss_currentHealth += hpRate;
        //Debug.Log(hpRate + " health regenerated from " + boss_currentHealth + " to " + (boss_currentHealth - hpRate));
        updateTheUi(boss_currentHealth);
    }

    IEnumerator flashHit()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        hit = false;
    }

    IEnumerator deadGuy()
    {
        while(spriteMat.GetFloat("_DissolvePower") != 0)
        {
            spriteMat.SetFloat("_DissolvePower", Mathf.Clamp((spriteMat.GetFloat("_DissolvePower") - (.1f * Time.deltaTime)), 0f, .99f));
            yield return new WaitForSeconds(.005f);
        }

        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

    void OnDisable()
    {
        //CancelInvoke();
        StopAllCoroutines();
    }

}
