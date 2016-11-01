using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

    public Transform canvas;
    public Button start;
    public Button exit;
    public Button credits;
    public Button back;

    private Animation cam;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Animation>();
    }

    void Update()
    {
        if (Input.GetKeyDown("escape") || Input.GetButtonDown("ESCAPE BUTTON"))
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        if(Input.GetButtonDown("StartButton"))
        {
            //Debug.Log("Starting game");
            SceneManager.LoadScene(1);
        }
    }

    public void ExitGame()
    {
        if (Input.GetButtonDown("ExitButton"))
        {
            //Debug.Log("Thanks for playing");
            Application.Quit();
        }

    }

    public void Credits()
    {
        if (Input.GetButtonDown("Credits"))
        {
            //Debug.Log("Enter credits");
            cam.Play("cameraAnim");
            EventSystem.current.SetSelectedGameObject(back.gameObject, null);
        }
    }

    public void Backbutton()
    {
        if(Input.GetButtonDown("Credits"))
        {
            cam.Play("camBackAnim");
            EventSystem.current.SetSelectedGameObject(start.gameObject, null);
        }
    }
}
