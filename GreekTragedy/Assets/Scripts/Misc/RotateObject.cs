using System;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class RotateObject : MonoBehaviour
{
    public static event Action<GameObject> OnObjectCreated;
    public bool isRotating = true;
    [SerializeField] Vector3 rotateVector;
    [SerializeField] Vector2 minMaxRotateSpeed;
    [SerializeField] bool randomizeRotation;
    float currentRotateSpeed;

    void Start() => OnObjectCreated?.Invoke(gameObject);

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
