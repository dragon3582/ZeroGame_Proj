using UnityEngine;
using System.Collections;

public class PowerUpShot : MonoBehaviour {

    public string type = "";
    public GameObject spread;
    public GameObject normal;
    public GameObject explosion;

    //could use this variable to control speeds of the bullets
    //public float speed;


    //assign the type to each prefab of the powerup in the scene
    
    //might need to be reworked later to account for dropping in the 
    //scene from enemies assuming when it's made it correctly assigns the type
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
            case "Explosion Power":
                type = "Explosion Shot";
                break;
        }
        Physics2D.IgnoreLayerCollision(8, 9);
    }

}
