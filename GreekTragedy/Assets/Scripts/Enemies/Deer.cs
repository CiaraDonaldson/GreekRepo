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
    SpriteRenderer _spriteRenderer;
    PlayerMove _playerMove;
    Vector2 _targetLocation;
    GameObject _player;
    float _currentAttack;
    bool _ableToAttack;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;
        _playerMove = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
    }

    private void OnEnable()
    {
        CurrentHealth = MaxHealth;
        SetTargetLocation();
        _currentAttack = attackRate;
        _player = GameObject.FindWithTag("Player");
        Invoke(nameof(CanAttack), spawnAttackDelay);
    }

    private void OnDisable() => CancelInvoke(nameof(CanAttack));

    void CanAttack() => _ableToAttack = true;

    private void FixedUpdate()
    {
        if (!_ableToAttack) return;
        _currentAttack = _currentAttack < 0 ? 0 : _currentAttack -= Time.deltaTime;

        if ((_targetLocation - (Vector2)transform.position).x < 0)
            _spriteRenderer.flipX = false;
        else if ((_targetLocation - (Vector2)transform.position).x > 0)
            _spriteRenderer.flipX = true;

        if (_player == null) return;
        _targetLocation = _player.transform.position;
        if (Vector3.Distance(_player.transform.position, transform.position) > attackDistance)
            transform.position = Vector2.MoveTowards((Vector2)transform.position, _targetLocation, moveRate);
        if (_currentAttack == 0 & Vector3.Distance(_player.transform.position, transform.position) <= attackDistance)
        {
            _currentAttack = attackRate;
            if (_player.TryGetComponent(out IDamagable damagable))
            {
                damagable.ApplyDamage(gameObject, attackDamage);
                if (_player.TryGetComponent(out PlayerMove move))
                    move.KnockBack((_player.transform.position - transform.position).normalized);
                OnAttackedTarget?.Invoke(gameObject);
            }
        }
    }


    void SetTargetLocation()
    {
        _targetLocation = new Vector2(Random.Range(-_playerMove.roomSize.x * .5f, _playerMove.roomSize.x * .5f), Random.Range(-_playerMove.roomSize.y * .5f, _playerMove.roomSize.y * .5f));
    }
}
