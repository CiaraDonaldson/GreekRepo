using darcproducts;
using UnityEngine;
using UnityEngine.Events;

public class Arrow : MonoBehaviour
{
    public int damage = 1;
    public Vector2 direction;
    public float speed;
    public LayerMask hitLayers;
    [SerializeField] GameEvent OnArrowHit;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Utilities.IsInLayerMask(collision.gameObject, hitLayers)) return;
        if (collision.TryGetComponent(out IDamagable damaged))
        {
            damaged.ApplyDamage(damage);
            OnArrowHit.Invoke(gameObject);
            gameObject.SetActive(false);
        }
    }
}
