using UnityEngine;
using System.Collections;

public class BossTrigger : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Camera.main.GetComponent<CameraFollow>().camMidPoint = true;
            this.gameObject.GetComponentInParent<BossAI>().enabled = true;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
