using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float maxSpeed = 9f;
    public GameObject fireDirection;
    public GameObject bullet;
    public float bulletSpeed;

    private Rigidbody2D _player;
    private bool _orientation = false;
    private Vector2 _currentDir;
    private string _shotType = "";

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

        if (moveX > .2)
        {
            _currentDir = direction;
            _orientation = true;
        }

        else if(moveX < -.2)
        {
            _currentDir = direction;
            _orientation = true;
        }


        if(moveY > .2)
        {
            _currentDir = direction;
            _orientation = false;
        }

        else if(moveY < -.2)
        {
            _currentDir = direction;
            _orientation = false;
        }


        if(_orientation)
        {
            fireDirection.transform.localPosition = _currentDir;
        }
        else if(!_orientation)
        {
            fireDirection.transform.localPosition = _currentDir;
        }

        if(_shotType == "")
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("normal ATTACK!");
                GameObject tempBullet;

                tempBullet = Instantiate(bullet, fireDirection.transform.position, fireDirection.transform.rotation) as GameObject;
                //Debug.Log("HI");

                Rigidbody2D tempRigid;
                tempRigid = tempBullet.GetComponent<Rigidbody2D>();

                tempRigid.AddForce(_currentDir * bulletSpeed * Time.deltaTime, ForceMode2D.Impulse);

                Physics2D.IgnoreCollision(tempBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                Physics2D.IgnoreLayerCollision(8, 8);

                Destroy(tempBullet, 3.0f);
            }
        }
        else if(_shotType == "SpreadShot")
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("SPREADSHOT ATTACK!");
                GameObject tempBullet;
                GameObject tempBullet2;

                tempBullet = Instantiate(bullet, fireDirection.transform.position, fireDirection.transform.rotation) as GameObject;

                Rigidbody2D tempRigid;
                tempRigid = tempBullet.GetComponent<Rigidbody2D>();

                tempRigid.AddForce(_currentDir * bulletSpeed * Time.deltaTime, ForceMode2D.Impulse);

                Physics2D.IgnoreCollision(tempBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                Physics2D.IgnoreLayerCollision(8, 8);

                Destroy(tempBullet, 3.0f);
            }
        }


    }

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
            Debug.Log("Yea you got the normal shot again.");
        }
    }
}
