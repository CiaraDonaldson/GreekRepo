using System.Collections;
using UnityEngine;


public class AimandCharge : Enemy
{
    public float rotationSpeed = 5f;
    public float launchSpeed = 10f;

    public int attackDamage = 1;

    private Transform playerTransform;
    private Vector3 launchTarget;
    public bool launching = false;

    public Color thisColor;
    public Renderer bodyRenderer;
    public SpriteRenderer _spriteRenderer;
    Vector2 _targetLocation;
    bool _hasHit;
    bool _hasLaunchPosition;
    public LayerMask hitLayers;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (playerTransform == null) return;
        if (!_hasLaunchPosition)
        {
            _hasLaunchPosition = true;
            launchTarget = playerTransform.position;
        }
        if ((_targetLocation - (Vector2)transform.position).x < 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if ((_targetLocation - (Vector2)transform.position).x > 0)
        {
            _spriteRenderer.flipX = true;
        }
        if ((_targetLocation - (Vector2)transform.position).y < 0)
        {
            _spriteRenderer.flipY = true;
        }
        else if ((_targetLocation - (Vector2)transform.position).y > 0)
        {
            _spriteRenderer.flipY = false;
        }

        bodyRenderer.material.color = thisColor;
        if (launching == false)
        {
            StartCoroutine(LaunchAfterWait());

            Vector3 targetDirection = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        else if (launching == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, launchTarget, launchSpeed * Time.deltaTime);
            if (transform.position == launchTarget)
            {
                ApplyDamage(gameObject, MaxHealth);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasHit) return;
        if (other.gameObject.TryGetComponent(out IDamagable success))
        {
            _hasHit = true;
            success.ApplyDamage(other.gameObject, attackDamage);
        }
    }


    IEnumerator LaunchAfterWait()
    {
        yield return new WaitForSeconds(3f);
        launching = true;
    }
}
