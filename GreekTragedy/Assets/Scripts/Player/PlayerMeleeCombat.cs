using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMeleeCombat : MonoBehaviour
{
    [SerializeField] KeyCode attackKey;
    [SerializeField] int attackDamage;
    [SerializeField] float meleeAttackDistance;
    [SerializeField] float meleeAttackRadius;
    [SerializeField] float attackDelay; // to prevent spamming
    [SerializeField] LayerMask hitLayers;
    [SerializeField] UnityEvent<GameObject> OnAttackMissed, OnAttackHit; // used for attack FX
    bool canAttack = true;
    Vector2 _attackPosition;
    Camera _cam;

    public Vector2 AttackPosition
    {
        get => _attackPosition;
        private set { }
    }

    void Start() => _cam = Camera.main;

    IEnumerator ResetAttack()
    {
        yield return new WaitForSecondsRealtime(attackDelay);
        canAttack = true;
    }

    void Update()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        _attackPosition = (Vector2)transform.position + (mousePos - (Vector2)transform.position).normalized * meleeAttackDistance;
        if (Input.GetKeyDown(attackKey))
            AttacKTargets();
    }

    void AttacKTargets()
    {
        if (canAttack)
        {
            canAttack = false;
            Collider2D[] hitTargets = Physics2D.OverlapCircleAll(_attackPosition, meleeAttackRadius, hitLayers);
            if (hitTargets.Length == 0)
            {
                OnAttackMissed?.Invoke(gameObject);
                StartCoroutine(ResetAttack());
                return;
            }
            foreach (var t in hitTargets)
            {
                if (t.TryGetComponent(out IDamagable success))
                    success.ApplyDamage(attackDamage);
            }
            OnAttackHit?.Invoke(hitTargets[0].gameObject);
            StartCoroutine(ResetAttack());
        }
    }


    private void OnDrawGizmos() // for visualizing location in scene vew during play
    {
        if (!canAttack) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_attackPosition, meleeAttackRadius);
    }
}
