using UnityEngine;

public sealed class RandomizeRotation2D : MonoBehaviour
{
    void OnEnable() => transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-180, 180)));
}
