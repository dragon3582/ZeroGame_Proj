using UnityEngine;
using System.Collections;

public class ParticleCollision : MonoBehaviour {

    Idamageable<float> damage;
    private float damageBit;

    void OnParticleCollision(GameObject other)
    {
        //Physics2D.IgnoreLayerCollision(8, 8);
        damage = (Idamageable<float>)other.gameObject.GetComponent(typeof(Idamageable<float>));
        damageBit = Mathf.Ceil(Random.Range(17f, 28f));
        if (other.gameObject.tag == "Enemy Box" || other.gameObject.tag == "Boss")
        {
            damage.takeDamage(damageBit, .7f, 3);
            //Debug.Log(damageBit);
            //Destroy(other.gameObject);
            //subtract miniscule amounts of health here
        }
    }

}
