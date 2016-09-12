using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float maxSpeed = 10f;

    private Rigidbody2D _player;

	// Use this for initialization
	void Awake ()
    {
        _player = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        _player.transform.position += transform.right * moveX * maxSpeed * Time.deltaTime;
        _player.transform.position += transform.up * moveY * maxSpeed * Time.deltaTime;
    }
}
