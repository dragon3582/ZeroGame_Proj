﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour, Idamageable<int> {

    public float maxSpeed = 13f;
    public GameObject fireDirection;
    public GameObject bullet;
    public GameObject hitCountGO;
    public GameObject currentPower;
    public Sprite[] icons;
    public string _shotType;
    public int _hitCount;
    public Sprite[] idles;
    public bool male;
    public bool female;
    public int bulletCount;

    #region private variables
    private AudioSource audioS;
    private GameObject shineGO;
    private float bulletSpeed = 30f;
    private Rigidbody2D _player;
    private bool _orientation = false;
    private Vector2 _currentDir;
    private float angle;
    private float timestamp;
    private float cooldownRate;
    private bool fireShot;
    //private Vector2 arcSpot;
    private Text _hitCountText;
    private Animator animateController;
    private SpriteRenderer flipper;
    private bool moving;
    private CamShake2 cam;
    private bool hit;
    #endregion

    
    void Awake ()
    {
        bulletCount = 0;
        audioS = GetComponent<AudioSource>();
        cam = Camera.main.gameObject.GetComponent<CamShake2>();
        _player = GetComponent<Rigidbody2D>();
        _hitCountText = hitCountGO.GetComponent<Text>();
        _hitCount = 0;
        updateHitCount();
        _shotType = "";
        animateController = GetComponentInChildren<Animator>();
        flipper = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        hit = false;
        shineGO = currentPower.transform.GetChild(0).gameObject;
	}
	
	
	void Update ()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(moveX, moveY);
        //moving = direction;
        //_player.transform.position += direction * maxSpeed * Time.deltaTime;
        //_player.transform.position += transform.up * moveY * maxSpeed * Time.deltaTime;

        _player.transform.Translate(direction * Time.deltaTime * maxSpeed);
        //_player.velocity = direction * Time.deltaTime * maxSpeed;

        if (moveX > .2 || moveX < -.2)
        {
             _currentDir = direction;
            _orientation = true;
            setOrientation();
            moving = true;
            animateController.enabled = true;
        }


        if(moveY > .2 || moveY < -.2)
        {
            _currentDir = direction;
            _orientation = false;
            setOrientation();
            moving = true;
            animateController.enabled = true;
        }

        if(moveX == 0 && moveY == 0)
        {
            moving = false;
        }
        //arcSpot = _currentDir * 2;
        if (!moving)
        {
            //animateController.SetBool("going up bool", false);
            //animateController.SetBool("going down bool", false);
            //animateController.SetBool("going right bool", false);
            //animateController.SetBool("going diagonal up bool", false);
            //animateController.SetBool("going diagonal down bool", false);
            animateController.enabled = false;
            if (_currentDir == Vector2.up || _currentDir == new Vector2(-1, 1) || _currentDir == Vector2.one)
            {
                if (female)
                {
                    flipper.sprite = idles[0];
                }
                else if(male)
                {
                    flipper.sprite = idles[3];
                }
            }

            else if (_currentDir == Vector2.left || _currentDir == Vector2.right)
            {
                if (female)
                {
                    flipper.sprite = idles[1];
                }
                else if (male)
                {
                    flipper.sprite = idles[4];
                }
            }

            else if(_currentDir == Vector2.down || _currentDir == new Vector2(1, -1) || _currentDir == -Vector2.one)
            {
                if (female)
                {
                    flipper.sprite = idles[2];
                }
                else if (male)
                {
                    flipper.sprite = idles[5];
                }
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            fireShot = true;
        }
        
        if(Input.GetKeyDown("escape") || Input.GetButtonDown("ESCAPE BUTTON"))
        {
            Application.Quit();
        }

        DontDestroyOnLoad(this.gameObject);

        if(hit)
        {
            StartCoroutine(flashHit());
        }

    }

    void FixedUpdate ()
    {
        //_player.velocity = moving * Time.deltaTime * maxSpeed;
        if (fireShot)
        {
            typeOfShot();
            fireShot = false;
        }
    }

    //pick up the powerups through the trigger
    //each powerup will assign the shot type based on what the type variable is in each powerup. same with
    //what bullet prefab is going to be shot. it will then destroy the powerup and set the current power icon 
    void OnTriggerEnter2D(Collider2D powerup)
    {
        if (powerup.tag == "Spreadshot Power")
        {
            StartCoroutine(shinePowerup());
            if(powerup.GetComponent<PowerUpShot>())
            {
                _shotType = powerup.GetComponent<PowerUpShot>().type;
                bullet = powerup.GetComponent<PowerUpShot>().spread;
            }
            Destroy(powerup.gameObject);
            currentPower.GetComponent<Image>().sprite = icons[1];
        }
        else if(powerup.tag == "Normal Shot")
        {
            StartCoroutine(shinePowerup());
            if (powerup.GetComponent<PowerUpShot>())
            {
                _shotType = powerup.GetComponent<PowerUpShot>().type;
                bullet = powerup.GetComponent<PowerUpShot>().normal;
            }
            Destroy(powerup.gameObject);
            currentPower.GetComponent<Image>().sprite = icons[0];
        }

        else if (powerup.tag == "Explosion Power")
        {
            StartCoroutine(shinePowerup());
            if (powerup.GetComponent<PowerUpShot>())
            {
                _shotType = powerup.GetComponent<PowerUpShot>().type;
                bullet = powerup.GetComponent<PowerUpShot>().explosion;
            }
            Destroy(powerup.gameObject);
            currentPower.GetComponent<Image>().sprite = icons[2];
        }
    }

    //coroutine to make the powerup shine after pickup

    IEnumerator shinePowerup()
    {
        audioS.Play();

        float t = 0.0f;
        float dur = .5f;

        Color temp;
        temp = shineGO.GetComponent<Image>().color;

        while (t < 1.0f)
        {
            t += Time.deltaTime / dur;
            temp.a = Mathf.Lerp(temp.a, 1f, t);
            shineGO.GetComponent<Image>().color = temp;
            yield return new WaitForSeconds(.015f);
        }

        t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime / dur;
            temp.a = Mathf.Lerp(temp.a, 0f, t);
            shineGO.GetComponent<Image>().color = temp;
            yield return new WaitForSeconds(.005f);
        }

    }

    //based on the type from the previous function above, it will shoot out in a certain pattern accordingly with a slight cooldown
    void typeOfShot()
    {

        //normal shot has a blank type
        if (_shotType == "")
        {
            //Debug.Log("normal attack...");
            GameObject tempBullet;
            // original cooldown was .2f
            cooldownRate = .02f;
            float torqueSpinFire = 10f;

            if (timestamp <= Time.time)
            {
                bulletCount++;
                timestamp = Time.time + cooldownRate;
                tempBullet = Instantiate(bullet, fireDirection.transform.position, fireDirection.transform.rotation) as GameObject;

                Rigidbody2D tempRigid;
                tempRigid = tempBullet.GetComponent<Rigidbody2D>();


                tempRigid.AddForce(_currentDir * bulletSpeed * Time.deltaTime, ForceMode2D.Impulse);
                tempRigid.AddTorque((bulletSpeed * torqueSpinFire) * Time.deltaTime, ForceMode2D.Impulse);

                Physics2D.IgnoreCollision(tempBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                Physics2D.IgnoreLayerCollision(8, 8);

                Destroy(tempBullet, 3.0f);
            }

        }

        //the spread shot attack powerup is in this area if the type was SpreadShot
        else if (_shotType == "SpreadShot")
        {
            //Debug.Log("SPREADSHOT ATTACK!");
            GameObject tempBullet;
            GameObject tempBullet2;
            GameObject tempBullet3;

            //finding the right angle to shoot the other two shots at was proving to be difficult by trying to find the angle
            //of the direction your currently facing. so i brute forced my way to make the angle since i already knew all the
            //possible shooting directions. aka hard coded coordinates 
            Vector2 offset = calcOffset(_currentDir , 1);
            Vector2 offset2 = calcOffset(_currentDir, 0);

            // original cooldown was .5f
            cooldownRate = .05f;

            if(timestamp <= Time.time)
            {
                bulletCount++;
                timestamp = Time.time + cooldownRate;
                float torqueSpin = 45f;

                tempBullet = Instantiate(bullet, fireDirection.transform.position, fireDirection.transform.rotation) as GameObject;
                tempBullet2 = Instantiate(bullet, fireDirection.transform.position, fireDirection.transform.rotation) as GameObject;
                tempBullet3 = Instantiate(bullet, fireDirection.transform.position, fireDirection.transform.rotation) as GameObject;

                Rigidbody2D tempRigid;
                Rigidbody2D tempRigid2;
                Rigidbody2D tempRigid3;
                tempRigid = tempBullet.GetComponent<Rigidbody2D>();
                tempRigid2 = tempBullet2.GetComponent<Rigidbody2D>();
                tempRigid3 = tempBullet3.GetComponent<Rigidbody2D>();

                tempRigid.AddForce(_currentDir *  bulletSpeed * Time.deltaTime, ForceMode2D.Impulse);
                tempRigid2.AddForce(offset * bulletSpeed * Time.deltaTime, ForceMode2D.Impulse);
                tempRigid3.AddForce(offset2 * bulletSpeed * Time.deltaTime, ForceMode2D.Impulse);

                tempRigid.AddTorque((bulletSpeed * torqueSpin) * Time.deltaTime, ForceMode2D.Impulse);
                tempRigid2.AddTorque((bulletSpeed * torqueSpin) * Time.deltaTime, ForceMode2D.Impulse);
                tempRigid3.AddTorque((bulletSpeed * torqueSpin) * Time.deltaTime, ForceMode2D.Impulse);

                Physics2D.IgnoreCollision(tempBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                Physics2D.IgnoreCollision(tempBullet2.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                Physics2D.IgnoreCollision(tempBullet3.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                Physics2D.IgnoreLayerCollision(8, 8);

                Destroy(tempBullet, .5f);
                Destroy(tempBullet2, .5f);
                Destroy(tempBullet3, .5f);
            }

        }

        //this is the explosion shot. it's relatively as simple as the normal shot because it sort of is, but it instantiates 
        //particles that do damage instead of this shot itself
        else if (_shotType == "Explosion Shot")
        {
            //Debug.Log("normal attack...");
            GameObject tempBullet;
            //GameObject tempPart;

            // original cooldown was .4f
            cooldownRate = .1f;

            if (timestamp <= Time.time)
            {
                bulletCount++;
                timestamp = Time.time + cooldownRate;
                
                tempBullet = Instantiate(bullet, fireDirection.transform.position, fireDirection.transform.rotation) as GameObject;

                float spin = 45f;
                Rigidbody2D tempRigid;
                tempRigid = tempBullet.GetComponent<Rigidbody2D>();

                tempRigid.AddForce(_currentDir * bulletSpeed * Time.deltaTime, ForceMode2D.Impulse);

                tempRigid.AddTorque((bulletSpeed * spin) * Time.deltaTime, ForceMode2D.Impulse);

                Physics2D.IgnoreCollision(tempBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                Physics2D.IgnoreLayerCollision(8, 8);

                Destroy(tempBullet, 2.0f);
                //tempPart = Instantiate(explosionParticles, tempBullet.transform.localPosition, tempBullet.transform.rotation) as GameObject;
                //Destroy(tempPart, 3f);

            }

        }
    }

    //this function was served to set the orientation of which direction you were currently facing, but in the long run it helped
    //serve as the function that set the animation bools.
    void setOrientation()
    {

        if(_currentDir == Vector2.up)
        {
            angle = 0f;
            animateController.enabled = true;
            animateController.SetBool("going up bool", true);
            animateController.SetBool("going down bool", false);
            animateController.SetBool("going right bool", false);
            animateController.SetBool("going diagonal up bool", false);
            animateController.SetBool("going diagonal down bool", false);

            flipper.flipX = false;
        }
        else if(_currentDir == Vector2.one)
        {
            angle = -45f;
            animateController.SetBool("going up bool", false);
            animateController.SetBool("going down bool", false);
            animateController.SetBool("going right bool", false);
            animateController.SetBool("going diagonal up bool", true);
            animateController.SetBool("going diagonal down bool", false);
            flipper.flipX = false;
        }
        else if(_currentDir == Vector2.right)
        {
            angle = -90f;
            animateController.SetBool("going up bool", false);
            animateController.SetBool("going down bool", false);
            animateController.SetBool("going right bool", true);
            animateController.SetBool("going diagonal up bool", false);
            animateController.SetBool("going diagonal down bool", false);
            flipper.flipX = false;
        }
        else if(_currentDir == new Vector2(1,-1))
        {
            angle = -135f;
            animateController.SetBool("going up bool", false);
            animateController.SetBool("going down bool", false);
            animateController.SetBool("going right bool", false);
            animateController.SetBool("going diagonal up bool", false);
            animateController.SetBool("going diagonal down bool", true);
            flipper.flipX = false;
        }
        else if(_currentDir == Vector2.down)
        {
            angle = -180f;
            animateController.SetBool("going up bool", false);
            animateController.SetBool("going down bool", true);
            animateController.SetBool("going right bool", false);
            animateController.SetBool("going diagonal up bool", false);
            animateController.SetBool("going diagonal down bool", false);
            flipper.flipX = false;
        }
        else if(_currentDir == -Vector2.one)
        {
            angle = -225f;
            animateController.SetBool("going up bool", false);
            animateController.SetBool("going down bool", false);
            animateController.SetBool("going right bool", false);
            animateController.SetBool("going diagonal up bool", false);
            animateController.SetBool("going diagonal down bool", true);
            flipper.flipX = true;
        }
        else if(_currentDir == Vector2.left)
        {
            angle = -270f;
            animateController.SetBool("going up bool", false);
            animateController.SetBool("going down bool", false);
            animateController.SetBool("going right bool", true);
            animateController.SetBool("going diagonal up bool", false);
            animateController.SetBool("going diagonal down bool", false);
            flipper.flipX = true;
        }
        else if (_currentDir == new Vector2(-1, 1))
        {
            angle = -315f;
            animateController.SetBool("going up bool", false);
            animateController.SetBool("going down bool", false);
            animateController.SetBool("going right bool", false);
            animateController.SetBool("going diagonal up bool", true);
            animateController.SetBool("going diagonal down bool", false);
            flipper.flipX = true;
        }


        if (_orientation)
        {
            fireDirection.transform.localPosition = _currentDir;
            fireDirection.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
        }
        else if (!_orientation)
        {
            fireDirection.transform.localPosition = _currentDir;
            fireDirection.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    //the brute forced/hard coded coordinate directional function i mentioned earlier
    Vector2 calcOffset(Vector2 ofs, int shot)
    {
        Vector2 lvs;
        if (ofs == Vector2.up)
        {
            lvs = new Vector2(0.3f, 1.3f);
            if (shot == 1)
            {
                return lvs;
            }
            else
            {
                return new Vector2(-0.3f, 1.3f);
            }
        }

        else if (ofs == Vector2.right)
        {
            lvs = new Vector2(1.3f, 0.3f);
            if (shot == 1)
            {
                return lvs;
            }
            else
            {
                return new Vector2(1.3f, -0.3f);
            }
        }

        else if (ofs == Vector2.left)
        {
            lvs = new Vector2(-1.3f, -0.3f);
            if (shot == 1)
            {
                return lvs;
            }
            else
            {
                return new Vector2(-1.3f, 0.3f);
            }
        }

        else if (ofs == Vector2.down)
        {
            lvs = new Vector2(0.3f, -1.3f);
            if (shot == 1)
            {
                return lvs;
            }
            else
            {
                return new Vector2(-0.3f, -1.3f);
            }
        }

        else if (ofs == Vector2.one)
        {
            lvs = new Vector2(1.3f, 0.7f);
            if (shot == 1)
            {
                return lvs;
            }
            else
            {
                return new Vector2(0.7f, 1.3f);
            }
        }

        else if (ofs == -Vector2.one)
        {
            lvs = new Vector2(-1.3f, -0.7f);
            if (shot == 1)
            {
                return lvs;
            }
            else
            {
                return new Vector2(-0.7f, -1.3f );
            }
        }

        else if (ofs == new Vector2(1, -1))
        {
            lvs = new Vector2(1.3f, -0.7f);
            if (shot == 1)
            {
                return lvs;
            }
            else
            {
                return new Vector2(0.7f, -1.3f);
            }
        }

        else if (ofs == new Vector2(-1, 1))
        {
            lvs = new Vector2(-1.3f, 0.7f);
            if (shot == 1)
            {
                return lvs;
            }
            else
            {
                return new Vector2(-0.7f, 1.3f);
            }
        }
        else
            return ofs;
    }

    //damage to increase the hitcount number
    public void takeDamage(int damageTaken, float nothin, int nothin2)
    {
        _hitCount += damageTaken;
        //Debug.Log(damageTaken + " damage the player took.");
        updateHitCount();
        cam.ShakeCamera(.35f, 1f);
        hit = true;
    }

    //function to update the UI hitcount
    void updateHitCount()
    {
        _hitCountText.text = _hitCount.ToString();
    }

    IEnumerator flashHit()
    {
        flipper.color = Color.red;
        yield return new WaitForSeconds(.1f);
        flipper.color = Color.white;
        yield return new WaitForSeconds(.1f);
        flipper.color = Color.red;
        yield return new WaitForSeconds(.1f);
        flipper.color = Color.white;
        hit = false;
    }
}
