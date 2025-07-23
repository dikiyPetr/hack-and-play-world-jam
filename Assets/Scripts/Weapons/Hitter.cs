using System;
using UnityEngine;

public class HitterObject : MonoBehaviour
{
    public static readonly String Tag = "Hitter";
    public float damage;

    public void hit(Damageable damageable)
    {
        damageable.TakeDamage(damage);
    }
}