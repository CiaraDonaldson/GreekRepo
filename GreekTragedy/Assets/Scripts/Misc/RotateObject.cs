using UnityEngine;
using Random = UnityEngine.Random;

public sealed class RotateObject : MonoBehaviour
{
    public bool isRotating = true;
    [SerializeField] Vector3 rotateVector;
    [SerializeField, Tooltip("Set both x and y to same value to not get random speeds")] Vector2 minMaxRotateSpeed;
    [SerializeField] bool randomizeRotation;
    float currentRotateSpeed;

    private void OnEnable()
    {
        if (randomizeRotation)
            rotateVector = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        currentRotateSpeed = Random.Range(minMaxRotateSpeed.x, minMaxRotateSpeed.y);
    }

    public void FixedUpdate()
    {
        if (isRotating)
            transform.Rotate(currentRotateSpeed * Time.deltaTime * rotateVector.normalized, Space.Self);
    }
}
