using darcproducts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AimandCharge : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float launchSpeed = 10f;

    public int attackDamage = 3;

    private Transform playerTransform;
    private Vector3 launchTarget;
    public bool launching = false;

    public Color thisColor;
    public Renderer bodyRenderer;
    public SpriteRenderer _spriteRenderer;
    Vector2 _targetLocation;

    public LayerMask hitLayers;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {

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

            launchTarget = playerTransform.position;
            
        }
        else if(launching == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, launchTarget, launchSpeed * Time.deltaTime);
            if (transform.position == launchTarget)
            {
                this.gameObject.TryGetComponent(out IDamagable success);
                success.ApplyDamage(this.gameObject, attackDamage);
            }
        }

    }
  
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.TryGetComponent(out IDamagable success);
        success.ApplyDamage(other.gameObject, attackDamage);
    }

    IEnumerator LaunchAfterWait()
    {
        yield return new WaitForSeconds(3f);
        launching = true;

    }
}
