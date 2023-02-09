using UnityEngine;

public sealed class RandomizeActivation : MonoBehaviour
{
    [Range(0f, 1f), SerializeField] private float acitvateChance;

    private void OnEnable()
    {
        if (Random.value < acitvateChance)
            gameObject.SetActive(false);
    }
}