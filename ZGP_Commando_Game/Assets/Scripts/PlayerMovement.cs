using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float maxSpeed = 9f;
    public GameObject fireDirection;
    public GameObject bullet;
    public float bulletSpeed;

    private Rigidbody2D _player;
    private Vector2 _currentDir;
    private bool _orientation = false;

	// Use this for initialization
	void Awake ()
    {
        _player = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

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


        if (Input.GetButtonDown("Fire1"))
        {
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
}
