using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Artemis : Enemy
{
    public enum BossState
    {
        Moving,
        MeleeAttack,
        RangedAttack,
        Enraged
    }

    [SerializeField] BossState currentState;
    [SerializeField] int arrowDamage = 1;
    [SerializeField] Slider healthBar;
    [SerializeField] float moveRate;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] int numberOfShots;
    [SerializeField] int numberOfShotsEnranged;
    [SerializeField] List<Transform> attackLocations = new();
    [SerializeField] UnityEvent<GameObject> OnHalfHealth, OnLowHealth;
    bool _firedHalfHealthEvent, _firedLowHealthEvent;
    Vector3 targetLocation;
    bool hasSetLocation;

    Animator anim;

    private void OnEnable()
    {
        CurrentHealth = MaxHealth;
        InitializeHealthBar();
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        switch (currentState)
        {
            case BossState.Moving:
                anim.SetTrigger("Run");
                if (!hasSetLocation)
                {
                    hasSetLocation = true;
                    targetLocation = attackLocations[Random.Range(0, attackLocations.Count)].position;
                }
                transform.Translate(moveRate * Time.deltaTime * (targetLocation - transform.position).normalized);
                if (Vector3.Distance(transform.position, targetLocation) < .4f)
                    currentState = BossState.RangedAttack;
                break;
            case BossState.RangedAttack:
                anim.SetTrigger("Attack");
                for (int i = 0; i < numberOfShots; ++i)
                {
                    Arrow arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity).GetComponent<Arrow>();
                    arrow.damage = arrowDamage;
                    arrow.direction = Random.insideUnitCircle.normalized;
                    //arrow.direction = new(-targetLocation.x, -targetLocation.y > 0 ? -targetLocation.y + i : -targetLocation.y - i);
                }
                hasSetLocation = false;
                if (CurrentHealth < MaxHealth / 4)
                    currentState = BossState.Enraged;
                else
                    currentState = BossState.Moving;
                break;
            case BossState.Enraged:
                numberOfShots = numberOfShotsEnranged;
                anim.SetTrigger("Run");
                if (!hasSetLocation)
                {
                    hasSetLocation = true;
                    targetLocation = attackLocations[Random.Range(0, attackLocations.Count)].position;
                }
                transform.Translate(moveRate * Time.deltaTime * 2 * (targetLocation - transform.position).normalized);
                if (Vector3.Distance(transform.position, targetLocation) < .4f)
                    currentState = BossState.RangedAttack;
                break;
        }
    }

    public void InitializeHealthBar()
    {
        healthBar.minValue = 0;
        healthBar.maxValue = MaxHealth;
        healthBar.value = MaxHealth;
    }

    public void UpdateHealthBar()
    {
        healthBar.value = CurrentHealth;
        if (healthBar.value < MaxHealth / 6 & !_firedLowHealthEvent)
        {
            _firedLowHealthEvent = true;
            OnLowHealth?.Invoke(gameObject);
        }
        else if (healthBar.value < MaxHealth / 2 & !_firedHalfHealthEvent)
        {
            _firedHalfHealthEvent = true;
            OnHalfHealth?.Invoke(gameObject);
        }
    }
}
