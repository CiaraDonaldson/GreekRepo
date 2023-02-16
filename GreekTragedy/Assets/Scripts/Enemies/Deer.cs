using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer : Enemy
{
    public float easing;
    public Vector2 minMaxRandomWait;
    Vector2 targetLocation;
    bool isGettingNewTargetLocation;

    private void Start()
    {
        SetTargetLocation();
    }

    IEnumerator DelayedTargetSet()
    {
        yield return new WaitForSecondsRealtime(Random.Range(minMaxRandomWait.x, minMaxRandomWait.y));
        SetTargetLocation();
        isGettingNewTargetLocation = false;
    }

    private void FixedUpdate()
    {
        switch (currentEnemyState)
        {
            case EnemyState.Moving:
                transform.position = Vector2.MoveTowards((Vector2)transform.position, targetLocation, easing);
                if ((Vector2)transform.position == targetLocation)
                {
                    if (!isGettingNewTargetLocation)
                    {
                        isGettingNewTargetLocation = true;
                        StartCoroutine(DelayedTargetSet());
                    }
                }
                break;
            case EnemyState.attacking:
                break;
            case EnemyState.Fleeing:
                break;
        }
    }

    void SetTargetLocation()
    {
        targetLocation = new Vector2(Random.Range(-PlayerMove.ROOM_SIZE.x * .5f, PlayerMove.ROOM_SIZE.x * .5f), Random.Range(-PlayerMove.ROOM_SIZE.y * .5f , PlayerMove.ROOM_SIZE.y * .5f));
    }
}
