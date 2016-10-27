using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossAI : MonoBehaviour, Idamageable<float> {

    public int speed = 2;
    public GameObject playerTarget;
    public GameObject bossBullet;
    public float timer;
    public float timeShot;
    public GameObject boss_healthCanvas;
    public GameObject cannon;

    private Vector2 direction = Vector2.right;
    private float boss_maxHealth = 1500f;
    private Image boss_fillHp;
    private float boss_currentHealth;
    private bool alive = true;
    private Color boss_alpha;
    private float waitTime = 1.5f;
    private GameObject directionGO;
    private Vector2 dir;

    void Start()
    {
        boss_currentHealth = boss_maxHealth;
        timer = Random.Range(2f, 7f);
        boss_fillHp = boss_healthCanvas.transform.GetChild(2).GetComponent<Image>();
        boss_alpha = this.gameObject.GetComponent<SpriteRenderer>().color;
        directionGO = cannon.transform.GetChild(0).gameObject;
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

        if (!alive)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            boss_alpha.a = .5f;
            this.GetComponent<SpriteRenderer>().color = boss_alpha;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            this.gameObject.GetComponent<BossAI>().enabled = false;

        }

    }

    void FixedUpdate()
    {
        fireShot();
    }

    void fireShot()
    {
        timeShot += Time.deltaTime;
        if (timeShot > waitTime)
        {
            dir = cannon.transform.position.normalized;
            Debug.Log(dir);
            GameObject tempBullet;

            tempBullet = Instantiate(bossBullet, cannon.transform.position, directionGO.transform.rotation) as GameObject;

            Rigidbody2D bullRb = tempBullet.GetComponent<Rigidbody2D>();
            bullRb.AddForce((directionGO.transform.position - cannon.transform.position) * Time.deltaTime, ForceMode2D.Impulse);
            //bullRb.AddForce(Vector2.right * Time.deltaTime, ForceMode2D.Impulse);
            //bullRig.velocity = test;
            Physics2D.IgnoreCollision(tempBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Destroy(tempBullet, 3.0f);

            timeShot = 0;
        }

    }

    public void takeDamage(float damageTaken)
    {
        Debug.Log(damageTaken + " damage the enemy took.");
        float previousHealth = boss_currentHealth;
        boss_currentHealth -= damageTaken;
        Debug.Log(boss_currentHealth);
        //StartCoroutine(updateTheUi(previousHealth, currentHealth));
        updateTheUi(previousHealth, boss_currentHealth);
    }

    void updateTheUi(float prevHP, float currentHP)
    {
        //float timer = 0f;
        currentHP = (currentHP / boss_maxHealth);
        prevHP = (prevHP / boss_maxHealth);
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

}
