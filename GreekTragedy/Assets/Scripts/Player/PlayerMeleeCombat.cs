using UnityEngine;

public class PlayerMeleeCombat : MonoBehaviour
{
    [SerializeField] float meleeAttackDistance;
    Vector2 attackPosition;
    Camera _cam;

    void Start() => _cam = Camera.main;

    void Update()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        attackPosition = (Vector2)transform.position + (mousePos - (Vector2)transform.position).normalized * meleeAttackDistance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPosition, .25f);
    }
}
