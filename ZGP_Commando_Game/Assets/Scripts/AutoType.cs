using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AutoType : MonoBehaviour
{
    private float letterPause = 0.02f;
    private Text textTyping;
    //public AudioClip sound;

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
        yield return new WaitForSeconds(1.3f);
        foreach (char letter in message.ToCharArray())
        {
            textTyping.text += letter;
            yield return 0;
            yield return new WaitForSeconds(letterPause);
        }

        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}