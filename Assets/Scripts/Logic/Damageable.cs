using System;

public class Damageable
{
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;
    public event Action<float> OnDamaged; // int = урон
    public event Action OnDeath;

    public Damageable(float maxHealth, float currentHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = ClampHealth(currentHealth);
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;
        OnDamaged?.Invoke(amount);
        CurrentHealth = ClampHealth(CurrentHealth - amount);
        if (IsDead)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (IsDead) return;

        CurrentHealth = ClampHealth(CurrentHealth + amount);
    }

    public void SetHealth(int newHealth)
    {
        CurrentHealth = ClampHealth(newHealth);
    }

    public void HealToFull()
    {
        CurrentHealth = MaxHealth;
    }

    private float ClampHealth(float value)
    {
        if (value < 0) return 0;
        if (value > MaxHealth) return MaxHealth;
        return value;
    }
}