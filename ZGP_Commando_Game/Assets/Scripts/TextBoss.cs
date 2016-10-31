using UnityEngine;
using System.Collections;

public class TextBoss : MonoBehaviour {

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
        }
    }
}
