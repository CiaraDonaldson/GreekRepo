using System.Collections;
using UnityEngine;

public class BombDeer : Enemy
{
    public Renderer bodyRenderer;
    [Range(0f, 1f)] public float colorChangeSpeed;
    public Color startColor, endColor;
    [Range(0f, .1f)] public float moveRate;
    public GameObject effect;

    public float radius = 50f;
    public int attackDamage;
    GameObject _player;
    Vector2 _targetLocation;
    SpriteRenderer _spriteRenderer;
    bool _hasExploded;
    bool _effectCreated;
    private float elapsedColorChangeTime = 0f;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (_player == null) return;
        _targetLocation = _player.transform.position;
        if ((_targetLocation - (Vector2)transform.position).x < 0)
            _spriteRenderer.flipX = false;
        else if ((_targetLocation - (Vector2)transform.position).x > 0)
            _spriteRenderer.flipX = true;
        transform.position = Vector2.MoveTowards((Vector2)transform.position, _targetLocation, moveRate);
    }

    void Update()
    {
        ChangeColor();
        if (bodyRenderer.material.color == endColor)
        {
            if (!_hasExploded)
            {
                _hasExploded = true;
                StartCoroutine(Explode());
            }
        }
    }
    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(1f);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D nearbyObject in hitColliders)
        {
            if (nearbyObject.gameObject.CompareTag("Player"))
            {
                if (nearbyObject.gameObject.TryGetComponent(out IDamagable damage))
                {
                    damage.ApplyDamage(nearbyObject.gameObject, attackDamage);
                    if (nearbyObject.TryGetComponent(out PlayerMove playerMove))
                        playerMove.KnockBack(nearbyObject.transform.position - transform.position);

                }
            }
        }
        if (!_effectCreated)
        {
            _effectCreated = true;
            Instantiate(effect, transform.position, Quaternion.identity);
        }
        if (gameObject.TryGetComponent(out IDamagable success))
            success.ApplyDamage(gameObject, MaxHealth);
    }

    private void ChangeColor()
    {
        if (bodyRenderer.material.color != endColor)
        {
            elapsedColorChangeTime += Time.deltaTime;
            float t = elapsedColorChangeTime / colorChangeSpeed;
            t = Mathf.Clamp01(t);
            bodyRenderer.material.color = Color.Lerp(startColor, endColor, t);
        }
    }
    private void OnDrawGizmosSelected() // for visualizing location in scene vew during play
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }
}

