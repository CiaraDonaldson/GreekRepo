using System.Collections;
using UnityEngine;


public class ArrowsFromPoints : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform[] arrowFiringPoints;
    [SerializeField] int arrowCount;
    [SerializeField] float delayBetweenArrows;
    bool _isFiring;

    public void FireArrows(int arrowCount)
    {
        if (arrowCount > 0)
            this.arrowCount = arrowCount;
        if (!_isFiring)
        {
            _isFiring = true;
            StartCoroutine(FireArrowVolly());
        }
    }


    public IEnumerator FireArrowVolly()
    {
        for (int i = 0; i < Mathf.Abs(arrowCount); i++)
        {
            int ranTransIndex = Random.Range(0, arrowFiringPoints.Length);
            GameObject arrowGO = Instantiate(arrowPrefab, arrowFiringPoints[ranTransIndex].position, Quaternion.identity, transform);
            if (arrowGO.TryGetComponent(out Arrow arrow))
                arrow.direction = arrowFiringPoints[ranTransIndex].position.x < 0 ? new Vector2(100, arrowFiringPoints[ranTransIndex].position.y).normalized : new Vector2(-100, arrowFiringPoints[ranTransIndex].position.y).normalized;
            yield return new WaitForSeconds(delayBetweenArrows);

        }
        _isFiring = false;
    }
}
