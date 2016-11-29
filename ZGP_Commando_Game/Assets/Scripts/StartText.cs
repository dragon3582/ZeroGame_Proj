using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartText : MonoBehaviour {

    private bool buttonCheck = true;

    void Update()
    {
        if(Input.anyKeyDown && buttonCheck)
        {
            StartCoroutine(startinDaGame());
            buttonCheck = false;
            //SceneManager.LoadScene(2);
        }
    }

    IEnumerator startinDaGame()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(2);
    }
}
