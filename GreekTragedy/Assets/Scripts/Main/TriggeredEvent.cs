using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class TriggeredEvent : MonoBehaviour
{
    [SerializeField] LayerMask triggerLayer;
    [SerializeField] UnityEvent<GameObject> OnTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!darcproducts.Utilities.IsInLayerMask(collision.gameObject, triggerLayer)) return;
        OnTriggered?.Invoke(gameObject);
    }
}
