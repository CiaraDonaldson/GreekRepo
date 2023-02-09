using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class ObjectPooler : MonoBehaviour
{
    public static event Action<GameObject> OnObjectSpawned;
    [SerializeField] GameObject objectToPool = null;
    readonly List<GameObject> objects = new();

    public Vector3 GetObjectSize() => objectToPool.transform.localScale;

    public GameObject GetObject()
    {
        if (objects.Count > 0)
        {
            for (int i = 0; i < objects.Count; i++)
                if (!objects[i].activeSelf)
                    return objects[i];
        }
        if (objectToPool != null)
        {
            GameObject newObject = Instantiate(objectToPool, Vector3.zero, Quaternion.identity, transform);
            newObject.SetActive(false);
            objects.Add(newObject);
            OnObjectSpawned?.Invoke(newObject);
            return newObject;
        }
        return null;
    }
}
