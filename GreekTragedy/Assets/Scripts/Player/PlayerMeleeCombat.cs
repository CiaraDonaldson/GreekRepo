using darcproducts;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerMeleeCombat : MonoBehaviour
{
    [SerializeField] KeyCode attackKey;
    [SerializeField] int attackDamage;
    [SerializeField] float meleeAttackDistance;
    [SerializeField] float meleeAttackRadius;
    [SerializeField] float attackDelay; // to prevent spamming
    [SerializeField] Image attackIndicator;
    [SerializeField] LayerMask hitLayers;
    [SerializeField] UnityEvent<GameObject> OnAttackHit; // used for attack FX
    [SerializeField] UnityEvent<Vector3> OnAttackMissed; // used for attack FX
    bool attackAvailable = true;
    bool isActive;
    float _attackTime;
    Vector2 _attackPosition;
    Camera _cam;

    public Vector2 AttackPosition
    {
        get => _attackPosition;
        private set { }
    }

    void Start()
    {
        _cam = Camera.main;
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
        _attackPosition = (Vector2)transform.position + (mousePos - (Vector2)transform.position).normalized * meleeAttackDistance;
        if (Input.GetKeyDown(attackKey))
            AttacKTargets();
    }

    void AttacKTargets()
    {
        if (!isActive) return;
        if (attackAvailable)
        {
            attackAvailable = false;
            _attackTime = 0;
            Collider2D hitTarget = Physics2D.OverlapCircle(_attackPosition, meleeAttackRadius, hitLayers);
            if (hitTarget == null)
            {
                OnAttackMissed?.Invoke(_attackPosition);
                StartCoroutine(ResetAttack());
                return;
            }
            if (hitTarget.TryGetComponent(out IDamagable success))
                success.ApplyDamage(attackDamage);
            OnAttackHit?.Invoke(hitTarget.gameObject);
            StartCoroutine(ResetAttack());
        }
    }


    private void OnDrawGizmos() // for visualizing location in scene vew during play
    {
        if (!attackAvailable) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_attackPosition, meleeAttackRadius);
    }
}
