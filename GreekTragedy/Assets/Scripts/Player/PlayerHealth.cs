using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public sealed class PlayerHealth : MonoBehaviour, IDamagable
{
    public static bool IsDead;
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
    bool canBeHit;
    bool _damageVignetteActive;
    private int _currentHealth;

    void Start()
    {
        ResetHealth();
        damageVignette.gameObject.SetActive(false);
        canBeHit = true;
    }

    /// <summary>
    /// Applies damage to enemy targets
    /// </summary>
    /// <param name="amount">Amount of damage you want to inflict</param>
    public void ApplyDamage(GameObject incObj, int amount)
    {
        if (canBeHit)
        {
            canBeHit = false;
            _currentHealth -= amount;
            _currentHealth = _currentHealth < 0 ? 0 : _currentHealth;
            OnPlayerDamaged?.Invoke(amount);
            if (_currentHealth == 0)
            {
                IsDead = true;
                OnPlayerDied?.Invoke(gameObject);
                Invoke(nameof(SendDelayedDiedEvent), delayedDiedEventTime);
                gameObject.SetActive(false);
            }
        }
    }

    public void ResetHealth()
    {
        IsDead = false;
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
        Vector2 pos = icon.transform.position;
        if (icon.TryGetComponent(out Image image))
            image.DOColor(new Color(1, 0, 0, 0), .2f);
        icon.transform.DOShakeRotation(.2f).SetEase(Ease.OutFlash);
        icon.transform.DOMoveY(pos.y - 1, .2f).SetEase(Ease.OutSine).OnComplete(() => { healthIcons.Remove(icon); DOTween.KillAll(); Destroy(icon); canBeHit = true; });
    }

    void SendDelayedDiedEvent() => OnPlayerDiedDelayed?.Invoke(gameObject);
}
