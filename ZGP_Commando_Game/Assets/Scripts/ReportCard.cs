using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReportCard : MonoBehaviour {

    public GameObject hitNum;
    public GameObject bulletNum;

    private GameObject camReset;
    private GameObject playerReset;
    private GameObject musicReset;
    private int hits;
    private int bullets;

    void Awake()
    {
        camReset = GameObject.Find("Camera");
        playerReset = GameObject.FindGameObjectWithTag("Player");
        musicReset = GameObject.Find("Music");

        hits = playerReset.GetComponent<PlayerMovement>()._hitCount;
        bullets = playerReset.GetComponent<PlayerMovement>().bulletCount;
        Destroy(camReset);
        Destroy(playerReset);
        Destroy(musicReset);
    }

    void Start()
    {
        hitNum.GetComponent<Text>().text = hits.ToString();
        bulletNum.GetComponent<Text>().text = bullets.ToString();
    }
	
	void Update ()
    {
        if (Input.GetButtonDown("ESCAPE BUTTON"))
        {
            Application.Quit();
        }
        else if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(0);
        }
	}
}
