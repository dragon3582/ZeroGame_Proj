using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AutoType : MonoBehaviour
{
    private float letterPause = 0.02f;
    private Text textTyping;
    public AudioClip sound;

    string message;

    // Use this for initialization
    void Start()
    {
        textTyping = this.gameObject.GetComponent<Text>();
        message = textTyping.text;
        textTyping.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        yield return new WaitForSeconds(2.0f);
        foreach (char letter in message.ToCharArray())
        {
            if(letter == '(')
            {
                letterPause = .05f;
            }
            textTyping.text += letter;
            if(sound && letter != ' ')
            {
                this.gameObject.GetComponent<AudioSource>().PlayOneShot(sound);
                yield return 0;
            }
            
            yield return new WaitForSeconds(letterPause);
        }

        yield return new WaitForSeconds(2.2f);
        Destroy(this.gameObject);
    }

}