using UnityEngine;
using System.Collections;

public class MusicAcrossScenes : MonoBehaviour {

    private static MusicAcrossScenes instance = null;

    public static MusicAcrossScenes Instance
    {
        get { return Instance; }
    }

    void Awake()
    {
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

    }
}
