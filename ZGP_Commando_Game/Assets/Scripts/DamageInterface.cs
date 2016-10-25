using UnityEngine;
using System.Collections;

public interface Idamageable<T>
{
    void takeDamage(T damageAmount);
}
