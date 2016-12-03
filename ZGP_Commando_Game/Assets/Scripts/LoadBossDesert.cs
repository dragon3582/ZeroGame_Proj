using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadBossDesert : MonoBehaviour
{

    private GameObject musicGO;

    void Awake()
    {
        musicGO = GameObject.FindGameObjectWithTag("Music");
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            switch(SceneManager.GetActiveScene().name)
            {
                case "MainSceneAD": loadDesertBossArena();
                    break;
                case "Desert Boss Scene": loadForestScene();
                    break;
                case "Forgetful forest": loadFinale();
                    break;


            }
            Camera.main.GetComponent<CameraFollow>().sceneChange = true;
            //SceneManager.LoadScene(3);
        }
    }

    void loadDesertBossArena()
    {
        SceneManager.LoadScene(3);
    }

    void loadForestScene()
    {
        musicGO.GetComponent<MusicAcrossScenes>().switchingAudio = true;
        SceneManager.LoadScene(4);
    }

    void loadFinale()
    {
        musicGO.GetComponent<MusicAcrossScenes>().switchingAudio = true;
        SceneManager.LoadScene(5);
    }
}
