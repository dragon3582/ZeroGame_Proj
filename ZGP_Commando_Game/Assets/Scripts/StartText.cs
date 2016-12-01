using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartText : MonoBehaviour {

    public GameObject timerToPlay;

    private float t = 0;
    private float duration = 4.0f;
    private GameObject progressBar;
    private Image bar;
    private bool buttonCheck = true;
    private bool barCheck;

    void Start()
    {
        progressBar = timerToPlay.transform.GetChild(1).gameObject;
        bar = progressBar.GetComponent<Image>();
    }

    void Update()
    {
        if(Input.anyKeyDown && buttonCheck)
        {
            buttonCheck = false;
            barCheck = true;
            timerToPlay.SetActive(true);
            StartCoroutine(startinDaGame());
        }
    }

    //coroutine to start the game with a correctly working loading bar
    IEnumerator startinDaGame()
    {
        
        while(t < 1f)
        {
            t += Time.deltaTime/duration;
            bar.fillAmount = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        if(bar.fillAmount == 1f)
        {
            SceneManager.LoadScene(2);
        }
    }
}
