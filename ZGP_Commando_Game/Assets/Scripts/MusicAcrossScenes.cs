using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicAcrossScenes : MonoBehaviour {

    public AudioClip desertTrack;
    public AudioClip forestTrack;
    public AudioClip forestFinale;
    public bool switchingAudio;

    private int count;
    private AudioSource audioVol;
    private static MusicAcrossScenes instance = null;

    public static MusicAcrossScenes Instance
    {
        get { return Instance; }
    }

    void Awake()
    {
        count = 1;
        audioVol = this.gameObject.GetComponent<AudioSource>();
        switchingAudio = false;
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Forgetful forest" && switchingAudio)
        {
            if(count == 1)
            {
                StartCoroutine(fadeOut());
            }
        }
        else if(SceneManager.GetActiveScene().name == "Forest Final Boss" && switchingAudio)
        {
            if (count == 1)
            {
                StartCoroutine(fadeOut());
            }
        }
    }

    IEnumerator fadeOut()
    {
        count--;
        float t = 0.0f;
        float dur = .5f;
        while(t < 1.0f)
        {
            t += Time.deltaTime/dur;
            audioVol.volume = Mathf.Lerp(audioVol.volume, .015f, t);
            yield return new WaitForSeconds(.05f);
        }
        audioVol.Stop();
        StartCoroutine(fadeIn());
    }

    IEnumerator fadeIn()
    {
        if(SceneManager.GetActiveScene().name == "Forgetful forest")
        {
            audioVol.clip = forestTrack;
        }
        else if(SceneManager.GetActiveScene().name == "Forest Final Boss")
        {
            audioVol.clip = forestFinale;
        }

        float t = 0.0f;
        float dur = .2f;

        while (t < 1.0f)
        {
            t += Time.deltaTime/dur;
            audioVol.volume = Mathf.Lerp(audioVol.volume, .7f, t);
            audioVol.Play();
            yield return new WaitForSeconds(.02f);
        }
        switchingAudio = false;
        count = 1;
    }
}
