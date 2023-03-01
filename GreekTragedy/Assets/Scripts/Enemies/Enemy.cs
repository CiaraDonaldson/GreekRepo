using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    public static event Action<GameObject> OnEnemyDamaged;
    public static event Action<GameObject> OnEnemyDied;
    public int MaxHealth;
    [SerializeField, Tooltip("Local events for self, can't reference scene objects if prefab")] 
    UnityEvent<GameObject> OnDamagedLocal;
    [SerializeField, Tooltip("Local events for self, can't reference scene objects if prefab")] 
    UnityEvent<GameObject> OnDiedLocal;
    private int _currentHealth;

    public int CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }

    public void ApplyDamage(int amount)
    {
        _currentHealth = _currentHealth - amount <= 0 ? 0 : _currentHealth -= amount;
        OnDamagedLocal?.Invoke(gameObject);
        OnEnemyDamaged?.Invoke(gameObject);
        if (_currentHealth != 0) return;
        OnDiedLocal?.Invoke(gameObject);
        OnEnemyDied?.Invoke(gameObject);
        gameObject.SetActive(false);
    }
}
