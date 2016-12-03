using UnityEngine;
using System.Collections;

public class ParticleCollision : MonoBehaviour {

    Idamageable<float> damage;
    private float damageBit;

    void OnParticleCollision(GameObject other)
    {
        damage = (Idamageable<float>)other.gameObject.GetComponent(typeof(Idamageable<float>));
        damageBit = Mathf.Ceil(Random.Range(10f, 22f));
        if (other.gameObject.tag == "Enemy Box" || other.gameObject.tag == "Boss")
        {
            damage.takeDamage(damageBit);
            //Debug.Log(damageBit);
            //Destroy(other.gameObject);
            //subtract miniscule amounts of health here
        }
    }
}
