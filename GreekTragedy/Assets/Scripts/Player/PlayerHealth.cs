using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] int maxHealth;
    [SerializeField] CanvasRenderer healthIconRenderer;
    [SerializeField] GameObject healthIconPrefab;
    List<GameObject> healthIcons = new();
    [SerializeField] UnityEvent<int> OnPlayerDamaged;
    [SerializeField] UnityEvent<GameObject> OnPlayerDied;
    [SerializeField] float delayedDiedEventTime;
    [SerializeField] CanvasRenderer damageVignette;
    [SerializeField] float damageVignetteTime;
    [SerializeField] UnityEvent<GameObject> OnPlayerDiedDelayed;
    bool _damageVignetteActive;
    private int _currentHealth;

    void Start()
    {
        ResetHealth();
        damageVignette.gameObject.SetActive(false);
    }

    /// <summary>
    /// Applies damage to enemy targets
    /// </summary>
    /// <param name="amount">Amount of damage you want to inflict</param>
    public void ApplyDamage(GameObject incObj, int amount)
    {
        _currentHealth -= amount;
        _currentHealth = _currentHealth < 0 ? 0 : _currentHealth;
        OnPlayerDamaged?.Invoke(amount);
        if (_currentHealth == 0)
        {
            OnPlayerDied?.Invoke(gameObject);
            Invoke(nameof(SendDelayedDiedEvent), delayedDiedEventTime);
            gameObject.SetActive(false);
        }
    }

    public void ResetHealth()
    {
        _currentHealth = maxHealth;
        for (int i = 0; i < maxHealth; i++)
            healthIcons.Add(Instantiate(healthIconPrefab, healthIconRenderer.transform));
    }

    public void ShowVignette() => StartCoroutine(ShowDamageVignette());

    IEnumerator ShowDamageVignette()
    {
        if (!_damageVignetteActive)
        {
            _damageVignetteActive = true;
            damageVignette.gameObject.SetActive(true);
            yield return new WaitForSeconds(damageVignetteTime);
            damageVignette.gameObject.SetActive(false);
            _damageVignetteActive = false;
        }
    }

    /// <summary>
    /// Removes health icon, used through OnPlayerDamaged event.
    /// </summary>
    public void RemoveHealthIcon()
    {
        if (healthIcons.Count == 0) return;
        GameObject icon = healthIcons[^1];
        healthIcons.Remove(icon);
        Destroy(icon);
    }

    void SendDelayedDiedEvent() => OnPlayerDiedDelayed?.Invoke(gameObject);
}
