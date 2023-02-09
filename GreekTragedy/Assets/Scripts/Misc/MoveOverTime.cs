using UnityEngine;

public sealed class MoveOverTime : MonoBehaviour
{
    [SerializeField] Vector2 minMaxSpeed;
    [SerializeField] Vector3 direction;
    float currentSpeed;

    private void OnEnable() => currentSpeed = Random.Range(minMaxSpeed.x, minMaxSpeed.y);

    public void FixedUpdate() => transform.position += currentSpeed * Time.deltaTime * direction.normalized;
}
