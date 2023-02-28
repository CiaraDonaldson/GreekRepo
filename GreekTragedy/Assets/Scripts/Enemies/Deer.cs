using UnityEngine;
using UnityEngine.Events;

public sealed class Deer : Enemy
{
    [Range(0f, .2f)] public float moveRate;
    public int attackDamage;
    public float attackRate;
    public float attackDistance;
    public Vector2 minMaxRandomWait;
    [SerializeField] float spawnAttackDelay = 1;
    [SerializeField] UnityEvent<GameObject> OnAttackedTarget;
    Vector2 targetLocation;
    GameObject _player;
    float _currentAttack;
    bool ableToAttack;

    private void OnEnable()
    {
        SetTargetLocation();
        _currentAttack = attackRate;
        _player = GameObject.FindWithTag("Player");
        Invoke(nameof(CanAttack), spawnAttackDelay);
    }

    private void OnDisable() => CancelInvoke(nameof(CanAttack));

    void CanAttack() => ableToAttack = true;

    private void FixedUpdate()
    {
        if (!ableToAttack) return;
        _currentAttack = _currentAttack < 0 ? 0 : _currentAttack -= Time.deltaTime;
        if (_player == null) return;
        targetLocation = _player.transform.position;
        if (Vector3.Distance(_player.transform.position, transform.position) > attackDistance)
            transform.position = Vector2.MoveTowards((Vector2)transform.position, targetLocation, moveRate);
        if (_currentAttack == 0 & Vector3.Distance(_player.transform.position, transform.position) <= attackDistance)
        {
            _currentAttack = attackRate;
            if (_player.TryGetComponent(out IDamagable damagable))
            {
                damagable.ApplyDamage(attackDamage);
                if (_player.TryGetComponent(out PlayerMove move))
                    move.KnockBack((_player.transform.position - transform.position).normalized);
                OnAttackedTarget?.Invoke(gameObject);
            }
        }
    }


    void SetTargetLocation()
    {
        targetLocation = new Vector2(Random.Range(-PlayerMove.ROOM_SIZE.x * .5f, PlayerMove.ROOM_SIZE.x * .5f), Random.Range(-PlayerMove.ROOM_SIZE.y * .5f, PlayerMove.ROOM_SIZE.y * .5f));
    }
}
