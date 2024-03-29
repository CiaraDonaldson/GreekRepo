using System.Collections;
using UnityEngine;


public class AimDeer : Enemy
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
    bool _reset;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        bodyRenderer.material.color = thisColor;
    }

    private void Disable()
    {
        CancelInvoke(nameof(ResetLaunchPostion));
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        if ((_targetLocation - (Vector2)transform.position).x < 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if ((_targetLocation - (Vector2)transform.position).x > 0)
        {
            _spriteRenderer.flipX = true;
        }
        if (launching == false)
        {
            StartCoroutine(LaunchAfterWait());

            /* Vector3 targetDirection = playerTransform.position - transform.position;
             float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
             Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
             transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);*/
        }
        else if (launching == true)
        {
            if (!_hasLaunchPosition)
            {
                _hasLaunchPosition = true;
                launchTarget = playerTransform.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, launchTarget, launchSpeed * Time.deltaTime);
            if (transform.position == launchTarget)
            {
                if (!_reset)
                {
                    _reset = true;
                    Invoke(nameof(ResetLaunchPostion), 3f);
                }
            }
        }
    }

    private void ResetLaunchPostion()
    {
        _hasLaunchPosition = false;
        _reset = false;
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
