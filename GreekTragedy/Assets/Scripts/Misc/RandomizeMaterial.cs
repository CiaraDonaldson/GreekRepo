using UnityEngine;

public sealed class RandomizeMaterial : MonoBehaviour
{
    [SerializeField] Material[] materials;
    Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void OnEnable()
    {
        int idx = Random.Range(0, materials.Length);
        if (materials[idx] != null & rend != null)
            rend.material = materials[idx];
    }
}
