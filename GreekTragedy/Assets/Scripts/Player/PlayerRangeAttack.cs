using darcproducts;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerRangeAttack : MonoBehaviour
{
    [SerializeField] KeyCode attackKey;
    [SerializeField] int attackDamage;
    [SerializeField] float attackDelay;
    [SerializeField] Image attackIndicator;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] UnityEvent<GameObject> OnFiredBow;
    bool attackAvailable = true;
    bool isActive;
    float _attackTime;
    Vector2 _attackDirection;
    Camera _cam;

    void Start()
    {
        _cam = Camera.main;
        SetAbleToAttack(true);
    }

    public void SetAbleToAttack(bool newValue) => isActive = newValue;

    IEnumerator ResetAttack()
    {
        yield return new WaitForSecondsRealtime(attackDelay);
        attackAvailable = true;
    }

    void Update()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        _attackTime = _attackTime > attackDelay ? attackDelay : _attackTime += Time.deltaTime;
        attackIndicator.fillAmount = Utilities.Remap(_attackTime, 0, attackDelay, 0, 1);
        _attackDirection = (mousePos - (Vector2)transform.position).normalized;
        if (Input.GetKeyDown(attackKey))
            FireBow();
    }

    void FireBow()
    {
        if (!isActive) return;
        if (Time.deltaTime == 0) return;
        if (attackAvailable)
        {
            attackAvailable = false;
            _attackTime = 0;
            GameObject arrowGO = Instantiate(arrowPrefab, (Vector2)transform.position, Quaternion.identity);
            if (arrowGO.TryGetComponent(out Arrow arrow))
            {
                arrow.damage = attackDamage;
                arrow.direction = _attackDirection;
                StartCoroutine(ResetAttack());
            }
        }
    }
}
