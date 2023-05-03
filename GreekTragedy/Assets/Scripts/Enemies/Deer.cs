using darcproducts;
using UnityEngine;
using UnityEngine.Events;

public sealed class Deer : Enemy
{
    [Range(0f, .2f)] public float moveRate;
    public LayerMask hitLayers;
    public int attackDamage;
    public float attackRate;
    public float attackDistance;
    public Vector2 minMaxRandomWait;
    [SerializeField] float spawnAttackDelay = 1;
    [SerializeField] float attackDelay = .3f;
    [SerializeField] UnityEvent<GameObject> OnAttackedTarget;
    SpriteRenderer _spriteRenderer;
    PlayerMove _playerMove;
    Vector2 _targetLocation;
    GameObject _player;
    float _currentAttack;
    bool _ableToAttack;
    bool _charging;
    float chargeDuration;
    public float chargeSpeed;
    Vector2 _chargeDirection;
    public float waitTime;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;
        _playerMove = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
    }

    private void OnEnable()
    {
        _spriteRenderer.enabled = false;
        CurrentHealth = MaxHealth;
        SetTargetLocation();
        _currentAttack = attackRate;
        _player = GameObject.FindWithTag("Player");
        Invoke(nameof(ShowDeer), spawnAttackDelay);
        Invoke(nameof(CanAttack), spawnAttackDelay);
    }

    private void OnDisable() => CancelInvoke(nameof(CanAttack));

    void CanAttack() => _ableToAttack = true;

    void ShowDeer() => _spriteRenderer.enabled = true;

    private void FixedUpdate()
    {
        if (!_ableToAttack) return;
        _currentAttack = _currentAttack < 0 ? 0 : _currentAttack -= Time.deltaTime;

        if (_charging)
        {
            chargeDuration -= Time.deltaTime;
            if (chargeDuration <= 0)
            {
                _charging = false;
                waitTime = Random.Range(minMaxRandomWait.x, minMaxRandomWait.y);
            }
            else
            {
                Vector2 newPos = (Vector2)transform.position + _chargeDirection * chargeSpeed * Time.deltaTime;
                newPos.x = Mathf.Clamp(newPos.x, -_playerMove.roomSize.x * .5f, _playerMove.roomSize.x * .5f);
                newPos.y = Mathf.Clamp(newPos.y, -_playerMove.roomSize.y * .5f, _playerMove.roomSize.y * .5f);
                transform.position = newPos;
            }
            return;
        }

        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }

        if ((_targetLocation - (Vector2)transform.position).x < 0)
            _spriteRenderer.flipX = false;
        else if ((_targetLocation - (Vector2)transform.position).x > 0)
            _spriteRenderer.flipX = true;

        if (_player == null) return;
        _targetLocation = _player.transform.position;

        if (Vector3.Distance(_player.transform.position, transform.position) <= attackDistance)
        {
            if (_currentAttack == 0)
            {
                _currentAttack = attackRate;
                _charging = true;
                chargeDuration = 1f;
                chargeSpeed = 5f;
                _chargeDirection = (_player.transform.position - transform.position).normalized;
            }
        }
        else
        {
            chargeSpeed = 1;
            transform.position = Vector2.MoveTowards((Vector2)transform.position, _targetLocation, moveRate);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!Utilities.IsInLayerMask(other.gameObject, hitLayers)) return;
        if (other.gameObject.TryGetComponent(out IDamagable damage) && _charging)
        {
            damage.ApplyDamage(other.gameObject, attackDamage);
            if (other.TryGetComponent(out PlayerMove playerMove))
                playerMove.KnockBack(other.transform.position - transform.position);
            _ableToAttack = false;
            Invoke(nameof(CanAttack), attackDelay);
        }
    }

    void SetTargetLocation()
    {
        _targetLocation = new Vector2(Random.Range(-_playerMove.roomSize.x * .5f, _playerMove.roomSize.x * .5f), Random.Range(-_playerMove.roomSize.y * .5f, _playerMove.roomSize.y * .5f));
    }
}