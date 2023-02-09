using UnityEngine;

public sealed class CreateObjectFromPool : MonoBehaviour
{
    public float ScaleMultiplier = 1;
    public bool IsActive = true;
    [SerializeField] Vector2Int minMaxAmount = Vector2Int.one;
    [SerializeField] Vector2 offset = Vector2.zero;
    [SerializeField] bool randomizeOffset;
    [SerializeField] float randomizeOffsetSize = 1;
    [SerializeField] private ObjectPooler objectToSpawn;
    private Vector3 originalScale;

    private void Start()
    {
        if (objectToSpawn != null)
            if (objectToSpawn.GetObject() != null)
                originalScale = objectToSpawn.GetObject().transform.localScale;
    }

    public void CreateNewObject(GameObject objLoc)
    {
        if (objectToSpawn == null | !IsActive) return;
        int newAmount = Random.Range(minMaxAmount.x, minMaxAmount.y + 1);
        for (int i = 0; i < newAmount; i++)
        {
            GameObject o = objectToSpawn.GetObject();
            if (o == null) return;
            if (!randomizeOffset)
                o.transform.position = objLoc.transform.position + (Vector3)offset;
            else
            {
                Vector3 newLoc = Random.onUnitSphere * randomizeOffsetSize;
                newLoc.z = 0;
                o.transform.position = objLoc.transform.position + newLoc;
            }
            o.transform.localScale = originalScale * ScaleMultiplier;
            o.SetActive(true);
        }
    }
}