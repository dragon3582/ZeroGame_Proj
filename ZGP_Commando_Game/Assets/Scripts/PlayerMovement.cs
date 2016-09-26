using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float maxSpeed = 9f;
    public GameObject fireDirection;
    public GameObject bullet;
    

    private float bulletSpeed = 30f;
    private Rigidbody2D _player;
    private bool _orientation = false;
    private Vector2 _currentDir;
    private Vector2 _previousDir;
    private float angle;
    private float timestamp;
    private float cooldownRate;
    private string _shotType = "";
    private bool fireShot;

	// Use this for initialization
    void Awake ()
    {
        _player = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(moveX, moveY);

        //_player.transform.position += direction * maxSpeed * Time.deltaTime;
        //_player.transform.position += transform.up * moveY * maxSpeed * Time.deltaTime;

        _player.transform.Translate(direction * Time.deltaTime * maxSpeed);

        if (moveX > .2 || moveX < -.2)
        {
            if (_currentDir != direction)
            {
                _previousDir = _currentDir;
            }
             _currentDir = direction;
            _orientation = true;
            setOrientation();
        }


        if(moveY > .2 || moveY < -.2)
        {
            if(_currentDir != direction)
            {
                _previousDir = _currentDir;
            }
            _currentDir = direction;
            _orientation = false;
            setOrientation();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            fireShot = true;
            //typeOfShot();
            //Debug.Log(_currentDir);
            //Debug.Log(_previousDir);
        }

    }

    void FixedUpdate ()
    {
        if(fireShot)
        {
            typeOfShot();
            fireShot = false;
        }
    }

    //pick up the powerups through the trigger
    void OnTriggerEnter2D(Collider2D powerup)
    {
        if(powerup.tag == "Spreadshot Power")
        {
            if(powerup.GetComponent<PowerUpShot>())
            {
                _shotType = powerup.GetComponent<PowerUpShot>().type;
                bullet = powerup.GetComponent<PowerUpShot>().spread;
            }
            Destroy(powerup.gameObject);
            Debug.Log("SPREADSHOT OBTAINED!");
        }
        else if(powerup.tag == "Normal Shot")
        {
            if(powerup.GetComponent<PowerUpShot>())
            {
                _shotType = powerup.GetComponent<PowerUpShot>().type;
                bullet = powerup.GetComponent<PowerUpShot>().normal;
            }
            Destroy(powerup.gameObject);
            //Debug.Log("Yea you got the normal shot again.");
        }
    }

    void typeOfShot()
    {

        if (_shotType == "")
        {
            //Debug.Log("normal attack...");
            GameObject tempBullet;
            cooldownRate = .2f;
            
            if(timestamp <= Time.time)
            {
                timestamp = Time.time + cooldownRate;
                tempBullet = Instantiate(bullet, fireDirection.transform.position, fireDirection.transform.rotation) as GameObject;

                Rigidbody2D tempRigid;
                tempRigid = tempBullet.GetComponent<Rigidbody2D>();


                tempRigid.AddForce(_currentDir * bulletSpeed * Time.deltaTime, ForceMode2D.Impulse);

                Physics2D.IgnoreCollision(tempBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                Physics2D.IgnoreLayerCollision(8, 8);

                Destroy(tempBullet, 3.0f);
            }

        }
        //handle spread shot logic here
        else if (_shotType == "SpreadShot")
        {
            //Debug.Log("SPREADSHOT ATTACK!");
            GameObject tempBullet;
            GameObject tempBullet2;
            GameObject tempBullet3;
            Vector2 offset = calcOffset(_currentDir , 1);
            Vector2 offset2 = calcOffset(_currentDir, 0);
            cooldownRate = .5f;

            if(timestamp <= Time.time)
            {
                timestamp = Time.time + cooldownRate;
                float torqueSpin = 50f;

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
    }

    void setOrientation()
    {

        if(_currentDir == Vector2.up)
        {
            angle = 0f;
        }
        else if(_currentDir == Vector2.one)
        {
            angle = -45f;
        }
        else if(_currentDir == Vector2.right)
        {
            angle = -90f;
        }
        else if(_currentDir == new Vector2(1,-1))
        {
            angle = -135f;
        }
        else if(_currentDir == Vector2.down)
        {
            angle = -180f;
        }
        else if(_currentDir == -Vector2.one)
        {
            angle = -225f;
        }
        else if(_currentDir == Vector2.left)
        {
            angle = -270f;
        }
        else if (_currentDir == new Vector2(-1, 1))
        {
            angle = -315f;
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

    //figuring out the angle was a bit difficult, so i brute forced my way to it since i already know the directions
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
}
