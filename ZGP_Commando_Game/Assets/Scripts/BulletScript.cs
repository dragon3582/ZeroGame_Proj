using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public GameObject playerRef;

    private GameObject _dir;

    private float _freq = 10f;
    private float _mag = .3f;
	

	// Update is called once per frame
	void Update ()
    {
        Vector2 pos;
        Vector2 wave = playerRef.transform.GetChild(1).position;

        pos = transform.position;

        if(this.tag == "Spread Bullet")
        {
            transform.position = pos + wave * Mathf.Cos(_freq * Time.time) * _mag;
        }
	}
}
