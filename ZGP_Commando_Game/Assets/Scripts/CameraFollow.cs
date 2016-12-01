using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour {



    public float dampTime = 0.15f;
    public bool sceneChange;

    private float cameraSize;
    private Vector3 velocity = Vector3.zero;
    private Scene activeScene;
    private Transform target;
    private Transform target2;
    private Vector3 point;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //activeScene = SceneManager.GetActiveScene();
    }

    void Start()
    {
        sceneChange = true;
        StartCoroutine(checkScene());
    }

    void LateUpdate()
    {
        
        if (target)
        {
            point = GetComponent<Camera>().WorldToViewportPoint(target.position);
            Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

        DontDestroyOnLoad(this.gameObject);

        if(sceneChange)
        {
            StartCoroutine(checkScene());
        }

    }

    public IEnumerator checkScene()
    {
        sceneChange = false;
        yield return new WaitForSeconds(1.5f);
        Debug.Log(SceneManager.GetActiveScene().name);
        if (GameObject.FindGameObjectWithTag("Boss"))
        {
            target2 = GameObject.FindGameObjectWithTag("Boss").transform;
            //Debug.Log("found it");
        }
        else if(!GameObject.FindGameObjectWithTag("Boss"))
        {
            target2 = null;
        }
    }
}
