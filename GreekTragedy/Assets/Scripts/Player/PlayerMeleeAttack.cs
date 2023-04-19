using darcproducts;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] KeyCode attackKey;
    [SerializeField] int attackDamage;
    [SerializeField] float meleeAttackDistance;
    [SerializeField] float meleeAttackRadius;
    [SerializeField] float attackDelay; // to prevent spamming
    [SerializeField] Image attackIndicator;
    [SerializeField] LayerMask hitLayers;
    [SerializeField] LayerMask bossLayer;
    [SerializeField] GameObject aimCursor;
    [SerializeField] GameEvent OnDeflectedArrow;
    [SerializeField] GameObject damageText;
    [SerializeField] Vector2 damageOffset = Vector2.zero;
    [SerializeField] UnityEvent<GameObject> OnAttackHit; // used for attack FX
    [SerializeField] UnityEvent<Vector3> OnAttackMissed; // used for attack FX
    PlayerRangeAttack playerRanged;
    bool attackAvailable = true;
    bool isActive;
    float _attackTime;
    Vector2 _attackPosition;
    Camera _cam;

    private void Awake() => playerRanged = GetComponent<PlayerRangeAttack>();

    public Vector2 AttackPosition
    {
        get => _attackPosition;
        private set { }
    }

    void Start()
    {
        _cam = Camera.main;
    }

    public void ShowDamage(GameObject location)
    {
        GameObject dmg = Instantiate(damageText, location.transform.position + (Vector3)damageOffset, Quaternion.identity);
        if (dmg.TryGetComponent(out TMP_Text text))
        {
            text.text = attackDamage.ToString();
        }
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
        aimCursor.transform.position = _attackPosition;
        if (Input.GetKeyDown(attackKey))
            AttacKTargets();
    }

    void AttacKTargets()
    {
        if (!isActive) return;
        if (Time.deltaTime == 0) return;
        if (attackAvailable)
        {
            attackAvailable = false;
            _attackTime = 0;
            Collider2D[] hitTargets = Physics2D.OverlapCircleAll(_attackPosition, meleeAttackRadius, hitLayers);
            if (hitTargets.Length == 0)
            {
                OnAttackMissed?.Invoke(_attackPosition);
                StartCoroutine(ResetAttack());
                return;
            }
            foreach (var t in hitTargets)
            {
                if (t.TryGetComponent(out IDamagable success))
                    success.ApplyDamage(gameObject, attackDamage);
                if (t.TryGetComponent(out Arrow arrow))
                {
                    arrow.hitLayers = bossLayer;
                    arrow.damage = attackDamage;
                    arrow.direction = (t.transform.position - transform.position).normalized * arrow.direction.magnitude;
                    OnDeflectedArrow.Invoke(gameObject);
                }
            }
            OnAttackHit?.Invoke(hitTargets[0].gameObject);
            StartCoroutine(ResetAttack());
        }
    }


    private void OnDrawGizmosSelected() // for visualizing location in scene vew during play
    {
        if (!attackAvailable) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_attackPosition, meleeAttackRadius);
    }
}
