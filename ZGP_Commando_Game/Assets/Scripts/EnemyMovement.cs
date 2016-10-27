using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour, Idamageable<float> {

    public GameObject enemyBullet;
    public Transform target;
    public bool alive;
    public GameObject healthCanvas;
    public float moveSpeed = 2.5f;

    private GameObject healthBar;
    private float xScale;
    private float distance;
    private bool seen;
    private bool running;
    private Vector2 test = Vector2.left;
    private float timer;
    private float waitTime = 1.5f;
    private float maxHealth = 100f;
    private float currentHealth;
    private Color alpha;
    private Image fillHp;
    private float lerpSpeed = 3.0f;

    void Awake()
    {
        xScale = transform.localScale.x;
        alive = true;
        currentHealth = maxHealth;
        alpha = this.gameObject.GetComponent<SpriteRenderer>().color;
        healthBar = healthCanvas.transform.GetChild(2).gameObject;
        fillHp = healthBar.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
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

            if(currentHealth <= 0)
            {
                alive = false;
            }
        }

        if(!alive)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            alpha.a = .5f;
            this.GetComponent<SpriteRenderer>().color = alpha;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            this.gameObject.GetComponent<EnemyMovement>().enabled = false;

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

    public void takeDamage(float damageTaken)
    {
        //Debug.Log(damageTaken + " damage the enemy took.");
        float previousHealth = currentHealth;
        currentHealth -= damageTaken;
        //StartCoroutine(updateTheUi(previousHealth, currentHealth));
        updateTheUi(previousHealth, currentHealth);
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

    void OnDisable()
    {
        StopAllCoroutines();
        //Debug.Log("I died");
    }
}
