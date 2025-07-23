using System;
using UnityEngine;
using VContainer;

public class HitterObject : MonoBehaviour
{
    public static readonly String Tag = "Hitter";
    public float damage;
    [Inject] private ActorsManager actorsManager;

    private void Start()
    {
        Di.instance.Inject(this);
    }

    public void hit(Damageable damageable)
    {
        damageable.TakeDamage(damage);
    }
}