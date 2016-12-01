using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadBossDesert : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            Camera.main.GetComponent<CameraFollow>().sceneChange = true;
            SceneManager.LoadScene(3);
        }
    }
}
