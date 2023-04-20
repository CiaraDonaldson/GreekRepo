using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    public static event Action<GameObject> OnEnemyDamaged;
    public static event Action<GameObject> OnEnemyDied;
    public int MaxHealth;
    [SerializeField] bool useTargetWhenDamaged;
    [SerializeField, Tooltip("Local events for self, can't reference scene objects if prefab")]
    GameEvent OnDamaged;
    [SerializeField, Tooltip("Local events for self, can't reference scene objects if prefab")]
    GameEvent OnDied;
    public int CurrentHealth;

    /// <summary>
    /// Uses IDamagable inteface to apply damage to enemy
    /// </summary>
    /// <param name="amount">Amount of damage applied</param>
    public void ApplyDamage(GameObject incObj, int amount)
    {
        CurrentHealth = CurrentHealth - amount <= 0 ? 0 : CurrentHealth -= amount;
        if (!useTargetWhenDamaged)
        {
            OnDamaged.Invoke(gameObject);
            OnEnemyDamaged?.Invoke(gameObject);
        }
        else
        {
            OnDamaged.Invoke(incObj);
            OnEnemyDamaged?.Invoke(incObj);
        }
        if (CurrentHealth != 0) return;
        OnDied.Invoke(gameObject);
        OnEnemyDied?.Invoke(gameObject);
        Destroy(gameObject);
    }
}
