using UnityEngine;
using System.Collections;

public class CamShake : MonoBehaviour
{

    public static CamShake instance;

    private Vector3 _originalPos;
    private float _timeAtCurrentFrame;
    private float _timeAtLastFrame;
    private float _fakeDelta;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // Calculate a fake delta time, so we can Shake while game is paused.
        _timeAtCurrentFrame = Time.realtimeSinceStartup;
        _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
        _timeAtLastFrame = _timeAtCurrentFrame;
    }

    public static void Shake(float duration, float amount)
    {
        instance._originalPos = instance.gameObject.transform.localPosition;
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.cShake(duration, amount));
    }

    
    public IEnumerator cShake(float duration, float amount)
    {
        float endTime = Time.time + duration;
        float damper = 1.0f - Mathf.Clamp(4.0f * endTime - 3.0f, 0.0f, 1.0f);

        while (duration > 0)
        {
            transform.localPosition = _originalPos + Random.insideUnitSphere * amount * damper;

            duration -= _fakeDelta;

            yield return null;
        }

        transform.localPosition = _originalPos;
    }
    


    //different method of shake that didn't work well at all
    /*
    public IEnumerator cShake(float duration, float magnitude)
    {

        float elapsed = 0.0f;

        //Vector3 originalCamPos = Camera.main.transform.localPosition;

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= magnitude * damper;
            y *= magnitude * damper;

            Camera.main.transform.localPosition = new Vector3(x, y, _originalPos.z);//originalCamPos.z);

            yield return null;
        }

        Camera.main.transform.localPosition = _originalPos; //originalCamPos;
    }
    */
}
