using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour, Idamageable<float> {

    [Header("Bullet being fired")]
    public GameObject enemyBullet;
    [Header("Alive variable")]
    public bool alive;
    public GameObject healthCanvas;
    public float moveSpeed = 2.5f;
    public GameObject[] powerups;
    public Sprite[] directions;

    #region private variables
    private Transform target;
    private int drop;
    private GameObject healthBar;
    //private float xScale;
    private float distance;
    private bool seen;
    private bool running;
    private int dropCount = 1;
    private float timer;
    // original wait time was 1f
    private float waitTime = .4f;
    private float maxHealth = 100f;
    private float currentHealth;
    private Color alpha;
    private Image fillHp;
    //private float lerpSpeed = 3.0f;
    private Animator animate;
    private SpriteRenderer flipper;
    private AudioSource grunt;
    private CamShake2 cam;
    #endregion


    void Awake()
    {
        //xScale = transform.localScale.x;
        cam = Camera.main.gameObject.GetComponent<CamShake2>();
        alive = true;
        currentHealth = maxHealth;
        alpha = this.gameObject.GetComponent<SpriteRenderer>().color;
        healthBar = healthCanvas.transform.GetChild(2).gameObject;
        fillHp = healthBar.GetComponent<Image>();
        animate = this.gameObject.GetComponent<Animator>();
        flipper = this.gameObject.GetComponent<SpriteRenderer>();
        grunt = this.gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        if (alive)
        {
            float move = moveSpeed * Time.deltaTime;

            distance = Vector2.Distance(this.transform.position, target.transform.position);
            //Debug.Log(distance);
            if(distance < 23f)
            {
                seen = true;
                animate.SetBool("insight", true);
                StartCoroutine(checkDist(move));
            }
            else if(distance > 25f)
            {
                seen = false;
                animate.SetBool("insight", false);
                StopCoroutine(checkDist(move));
            }

            if(currentHealth <= 0)
            {
                alive = false;
            }
        }

        if(!alive && dropCount == 1)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            alpha.a = .5f;
            this.GetComponent<SpriteRenderer>().color = alpha;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            animate.enabled = false;

            drop = Random.Range(0, 6);

            GameObject tempPower;

            switch(drop)
            {
                case 0: tempPower = Instantiate(powerups[0], this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
                        Destroy(tempPower, 5.0f);
                    break;
                case 1: tempPower = Instantiate(powerups[1], this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
                        Destroy(tempPower, 5.0f);
                    break;
                case 2: tempPower = Instantiate(powerups[2], this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
                        Destroy(tempPower, 5.0f);
                    break;
            }
            this.gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
            dropCount = 0;
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
        if(seen && alive)
        {
            fireShot();
        }

        if(!alive)
        {
            flyDeath();
        }
    }

    IEnumerator checkDist(float move)
    {
        if(seen)
        {
            Vector3 dirWay = (transform.position - target.position).normalized;
            float distanceCheck = Vector2.Distance(this.transform.position, target.transform.position);
            if (distanceCheck > 10f)
                transform.position = Vector3.MoveTowards(transform.position, target.position, move);
            //Debug.Log(dirWay);
            if(dirWay.x < 0)
            {
                flipChar(true);
            }
            else if(dirWay.x > 0)
            {
                flipChar(false);
            }
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

            bullRig.AddForce((target.transform.position - transform.position).normalized * Time.deltaTime, ForceMode2D.Impulse);
            //bullRig.velocity = test;
            Physics2D.IgnoreCollision(tempBul.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Destroy(tempBul, 3.0f);

            timer = 0;
        }

    }

    public void takeDamage(float damageTaken)
    {
        //Debug.Log(damageTaken + " damage the enemy took.");
        float previousHealth = currentHealth;
        currentHealth -= damageTaken;
        //StartCoroutine(updateTheUi(previousHealth, currentHealth));
        updateTheUi(previousHealth, currentHealth);
        grunt.Play();
        //CamShake.Shake(.5f, .8f);
        //cam.ShakeCamera(.5f, 1f);
    }

    void updateTheUi(float prevHP, float currentHP)
    {
        //float timer = 0f;
        currentHP = (currentHP / maxHealth);
        prevHP = (prevHP / maxHealth);
        /*
        if (timer < lerpSpeed)
        {
            fillHp.fillAmount = Mathf.Lerp(fillHp.fillAmount, currentHP, (lerpSpeed/timer));
            timer += Time.deltaTime;
        }
        */
        fillHp.fillAmount = currentHP;
        //yield return new WaitForEndOfFrame();
    }

    void flipChar(bool facing)
    {
        if (facing)
        {
            flipper.flipX = true;
        }
        else if (!facing)
        {
            flipper.flipX = false;
        }
    }

    void flyDeath()
    {
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 2500f * Time.deltaTime, ForceMode2D.Force);
        this.gameObject.GetComponent<Rigidbody2D>().AddTorque(23f * Time.deltaTime, ForceMode2D.Impulse);
        transform.Rotate(Vector3.up * 300f * Time.deltaTime, Space.World);
        //Vector3.Lerp(transform.rotation.y, transform.rotation.y * 180, .5f);
        //yield return new WaitForSeconds(3.5f);
        //this.gameObject.GetComponent<EnemyMovement>().enabled = false;

        Destroy(this.gameObject, 3.5f);
    }

    void OnDisable()
    {
        StopAllCoroutines();
        //Debug.Log("I died");
    }
}
