using UnityEngine;
using System.Collections;

public interface Idamageable<T>
{
    void takeDamage(T damageAmount, float damageMul = 2.0f, int bullet = 1);
}