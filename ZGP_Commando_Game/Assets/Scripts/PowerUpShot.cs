using UnityEngine;
using System.Collections;

public class PowerUpShot : MonoBehaviour {

    public string type = "";
    public GameObject spread;
    public GameObject normal;

    void Awake ()
    {
        switch (this.tag)
        {
            case "Spreadshot Power":
                type = "SpreadShot";
                break;
            case "Normal Power":
                type = "";
                break;
        }
    }

}
