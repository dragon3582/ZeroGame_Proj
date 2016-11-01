using UnityEngine;
using System.Collections;

public class StartText : MonoBehaviour {

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            this.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
