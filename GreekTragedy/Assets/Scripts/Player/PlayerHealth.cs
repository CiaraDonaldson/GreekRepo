using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] int maxHealth;
    [SerializeField] CanvasRenderer healthIconRenderer;
    [SerializeField] GameObject healthIconPrefab;
    List<GameObject> healthIcons = new();
    [SerializeField] UnityEvent<int> OnPlayerDamaged;
    [SerializeField] UnityEvent<GameObject> OnPlayerDied;
    private int _currentHealth;

    void Start()
    {
        _currentHealth = maxHealth;
        for (int i = 0; i < maxHealth; i++)
        {
            healthIcons.Add(Instantiate(healthIconPrefab, healthIconRenderer.transform));
        }
    }

    public void ApplyDamage(int amount)
    {
        _currentHealth = _currentHealth -= amount < 0 ? 0 : _currentHealth -= amount;
        OnPlayerDamaged?.Invoke(amount);
        if (_currentHealth == 0)
        {
            OnPlayerDied?.Invoke(gameObject);
        }
    }

    public void RemoveHealthIcon()
    {
        if (healthIcons.Count == 0) return;
        GameObject icon = healthIcons[healthIcons.Count - 1];
        healthIcons.Remove(icon);
        DestroyImmediate(icon);
    }
}
