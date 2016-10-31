using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Transform canvas;
    public Button start;
    public Button exit;
    public Button credits;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void StartGame()
    {
        if(Input.GetButton("StartButton"))
        {
            Debug.Log("Starting game");
            SceneManager.LoadScene(1);
        }
    }

    public void ExitGame()
    {
        if (Input.GetButton("ExitButton"))
        {
            Debug.Log("Thanks for playing");
            Application.Quit();
        }

    }

    public void Credits()
    {
        if (Input.GetButton("Credits"))
        {
            Debug.Log("Enter credits");
            SceneManager.LoadScene("Credits");
        }
    }
}
