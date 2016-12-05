using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartText : MonoBehaviour {

    public GameObject timerToPlay;
    public GameObject pressTextGO;

    private float t = 0;
    private float duration = 2.5f;
    private GameObject progressBar;
    private Image bar;
    private bool buttonCheck = true;
    private bool barCheck;
    private bool readText;
    private string pressText;

    void Start()
    {
        pressText = pressTextGO.GetComponent<Text>().text;
        pressTextGO.GetComponent<Text>().text = "Please wait...";
        readText = false;
        progressBar = timerToPlay.transform.GetChild(1).gameObject;
        bar = progressBar.GetComponent<Image>();
        StartCoroutine(readTitle());
    }

    void Update()
    {
        if (Input.GetButtonDown("ESCAPE BUTTON"))
        {
            Application.Quit();
        }
        else if (Input.anyKeyDown && buttonCheck && readText)
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

    IEnumerator readTitle()
    {
        yield return new WaitForSeconds(3.0f);
        pressTextGO.GetComponent<Text>().text = pressText;
        readText = true;
    }
}
