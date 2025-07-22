using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyScript : MonoBehaviour
{
    private class Animation
    {
        internal static readonly int Dead = Animator.StringToHash("Dead");
        internal static readonly int Hit = Animator.StringToHash("Hit");
    }

    private int id;
    public float hp = 1f;
    public float moveSpeed = 1f;
    public Vector2 target = new(0.5f, 0.5f);
    private SpriteRenderer _sprite;
    private Animator _animator;

    public void Hit(float damage)
    {
        if (hp > 0)
        {
            _animator.SetBool(Animation.Hit, true);
            hp -= damage;
            if (hp <= 0)
            {
                Dead();
            }
        }
    }

    public void Dead()
    {
        _animator.SetBool(Animation.Dead, true);
    }

    void Start()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (hp > 0)
        {
            Move(target);
        }
    }

    private void Move(Vector2 targetPosition)
    {
        Vector2 currentPosition = transform.position;

        Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
        _sprite.flipX = newPosition.x < currentPosition.x;

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        //debug
        if (targetPosition == newPosition)
        {
            Hit(1);
        }
    }

    public void SetId(int newId)
    {
        id = newId;
    }

    public int GetId()
    {
        return id;
    }
}