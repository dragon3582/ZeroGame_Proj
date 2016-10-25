using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

    public delegate void onDamageTaken();
    public static event onDamageTaken damageTarget;
}
