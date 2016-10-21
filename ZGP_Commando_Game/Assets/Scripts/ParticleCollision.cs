using UnityEngine;
using System.Collections;

public class ParticleCollision : MonoBehaviour {

    void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.tag == "Enemy Box")
        {
            Destroy(other.gameObject);
            //subtract miniscule amounts of health here
        }
    }
}
