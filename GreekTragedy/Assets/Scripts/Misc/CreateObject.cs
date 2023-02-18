using UnityEngine;

public sealed class CreateObject : MonoBehaviour
{
    public bool IsActive = true;
    [SerializeField] Vector2 minMaxScaleMultiplier;
    [SerializeField] Vector2Int minMaxAmountToSpawn = Vector2Int.one;
    [SerializeField] Vector2 offset = Vector2.zero;
    [SerializeField] bool randomizeOffset;
    [SerializeField] float randomizeOffsetSize = 1;
    [SerializeField] private GameObject objectToSpawn;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = objectToSpawn.transform.localScale;
    }

    public void CreateNewObject(GameObject objLoc)
    {
        CreateNewObject(objLoc.transform.position);
    }

    public void CreateNewObject(Vector3 objLoc)
    {
        if (objectToSpawn == null | !IsActive) return;
        int newAmount = Random.Range(minMaxAmountToSpawn.x, minMaxAmountToSpawn.y + 1);
        for (int i = 0; i < newAmount; i++)
        {
            GameObject o = Instantiate(objectToSpawn);
            if (!randomizeOffset)
                o.transform.position = objLoc + (Vector3)offset;
            else
            {
                Vector3 newLoc = Random.onUnitSphere * randomizeOffsetSize;
                newLoc.z = 0;
                o.transform.position = objLoc + newLoc;
            }
            o.transform.localScale = originalScale * Random.Range(minMaxScaleMultiplier.x, minMaxScaleMultiplier.y);
            o.SetActive(true);
        }
    }
}
