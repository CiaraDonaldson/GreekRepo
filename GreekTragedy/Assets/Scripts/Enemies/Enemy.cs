using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    public static event Action<GameObject> OnEnemyDamaged;
    public static event Action<GameObject> OnEnemyDied;
    public int maxHealth;
    public float moveSpeed;
    private float _currentHealth;

    void OnEnable() => _currentHealth = maxHealth;

    public void ApplyDamage(int amount)
    {
        _currentHealth = _currentHealth -= amount == 0 ? 0 : _currentHealth -= amount;
        OnEnemyDamaged?.Invoke(gameObject);
        if (_currentHealth != 0) return;
        OnEnemyDied?.Invoke(gameObject);
    }
}
