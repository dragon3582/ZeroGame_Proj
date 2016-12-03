using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour {



    public float dampTime = 0.15f;
    public bool sceneChange;
    public bool camMidPoint;

    private float cameraSize;
    private Vector3 velocity = Vector3.zero;
    private Scene activeScene;
    private Transform target;
    private Transform target2;
    private Vector3 point;
    private Vector3 point2;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //activeScene = SceneManager.GetActiveScene();
    }

    void Start()
    {
        sceneChange = true;
        camMidPoint = false;
        StartCoroutine(checkScene());
    }

    void LateUpdate()
    {
        
        if (target && !camMidPoint)
        {
            point = GetComponent<Camera>().WorldToViewportPoint(target.position);
            Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
        else if (target && target2 && camMidPoint)
        {
            point = GetComponent<Camera>().WorldToViewportPoint(target.position);
            Vector3 delta = (target.position - target2.position) - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            Vector3 center = ((target2.position - target.position) / 2.0f) + target.position - new Vector3(0, 0, 42.25f);
            float distanceBetween = Vector3.Distance(target.position, target2.position);
            GetComponent<Camera>().orthographicSize = distanceBetween/2.0f;
            //Debug.Log(distanceBetween);
            transform.position = Vector3.SmoothDamp(transform.position, center, ref velocity, dampTime);
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
            target2 = GameObject.FindGameObjectWithTag("Boss").transform.GetChild(4);
            StartCoroutine(lookAtBoss());
            //Debug.Log("found it");
        }
        else if(!GameObject.FindGameObjectWithTag("Boss"))
        {
            target2 = null;
        }
    }

    IEnumerator lookAtBoss()
    {
        Transform temp;
        temp = target;

        dampTime = .8f;
        target = target2;

        target2.transform.parent.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
        yield return new WaitForSeconds(6.6f);
        target = temp;
        yield return new WaitForSeconds(.6f);
        dampTime = 0.15f;
    }
}
