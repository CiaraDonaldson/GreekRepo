using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    public static event Action<GameObject> OnEnemyDamaged;
    public static event Action<GameObject> OnEnemyDied;
    public int maxHealth;
    private float _currentHealth;

    void OnEnable() => _currentHealth = maxHealth;

    public void ApplyDamage(int amount)
    {
        _currentHealth -= amount;
        if (_currentHealth < 0) 
            _currentHealth = 0;
        print($"Enemy Damaged: {gameObject.name} for {amount} points of health!");
        OnEnemyDamaged?.Invoke(gameObject);
        if (_currentHealth != 0) return;
        OnEnemyDied?.Invoke(gameObject);
        gameObject.SetActive(false);
    }
}
