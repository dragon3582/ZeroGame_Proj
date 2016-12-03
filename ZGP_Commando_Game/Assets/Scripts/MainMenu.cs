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
    public Button joe;
    public Button alice;
    public GameObject musicGO;

    private Animation cam;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Animation>();
        back.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown("escape") || Input.GetButtonDown("ESCAPE BUTTON"))
        {
            Application.Quit();
        }
    }

    public void ChooseCharacter()
    {
        joe.gameObject.SetActive(true);
        alice.gameObject.SetActive(true);
        start.gameObject.SetActive(false);
        back.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(alice.gameObject, null);
    }

    public void JoeStartGame()
    {
        musicGO.GetComponent<MusicAcrossScenes>().characterString = "Joe";
        musicGO.GetComponent<MusicAcrossScenes>().choseCharacter = true;
        SceneManager.LoadScene(1);
    }

    public void AliceStartGame()
    {
        musicGO.GetComponent<MusicAcrossScenes>().characterString = "Alice";
        musicGO.GetComponent<MusicAcrossScenes>().choseCharacter = true;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        //Debug.Log("Thanks for playing");
        Application.Quit();

    }

    public void Credits()
    {
        //Debug.Log("Enter credits");
        cam.Play("cameraAnim");
        back.enabled = true;
        EventSystem.current.SetSelectedGameObject(back.gameObject, null);
        start.enabled = false;
        exit.enabled = false;
        credits.enabled = false;
    }

    public void Backbutton()
    {
        cam.Play("camBackAnim");
        start.enabled = true;
        exit.enabled = true;
        credits.enabled = true;
        EventSystem.current.SetSelectedGameObject(start.gameObject, null);
        back.enabled = false;
    }
}
