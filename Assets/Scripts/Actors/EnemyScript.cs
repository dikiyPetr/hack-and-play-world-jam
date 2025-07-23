using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class EnemyScript : MonoBehaviour
{
    public static readonly String Tag = "Enemy";

    private class Animation
    {
        internal static readonly int Dead = Animator.StringToHash("Dead");
        internal static readonly int Hit = Animator.StringToHash("Hit");
    }

    private Damageable damageable;
    private int id;
    public float hp = 1f;
    public float moveSpeed = 1f;
    public Vector2 target = new(0.5f, 0.5f);
    private SpriteRenderer _sprite;
    private Animator _animator;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();

        // Внимание: нельзя инициализировать damageable до OnEnable, если ты хочешь переиспользовать объект
    }

    void OnEnable()
    {
        damageable = new Damageable(hp, hp);
        damageable.OnDamaged += OnDamaged;
        damageable.OnDeath += OnDeath;
    }

    void OnDisable()
    {
        if (damageable != null)
        {
            damageable.OnDamaged -= OnDamaged;
            damageable.OnDeath -= OnDeath;
        }
    }

    void FixedUpdate()
    {
        if (damageable.IsDead)
            return;

        Move(target);
    }

    private void Move(Vector2 targetPosition)
    {
        Vector2 currentPosition = rb.position;

        Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.fixedDeltaTime);
        _sprite.flipX = newPosition.x < currentPosition.x;

        rb.MovePosition(newPosition);
    }

    public void SetId(int newId)
    {
        id = newId;
    }

    public int GetId()
    {
        return id;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(HitterObject.Tag))
        {
            other.GetComponent<HitterObject>().hit(damageable);
        }
    }

    void OnDeath()
    {
        rb.simulated = false;
        _animator.SetBool(Animation.Dead, true);
    }

    void OnDamaged(float damage)
    {
        _animator.SetBool(Animation.Hit, true);
    }
}