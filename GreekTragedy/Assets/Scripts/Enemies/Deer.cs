using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer : Enemy
{
    public float easing;
    Vector2 targetLocation;

    private void Start()
    {
        SetTargetLocation();
    }

    private void Update()
    {
        switch (currentEnemyState)
        {
            case EnemyState.Moving:
                transform.position = Vector3.MoveTowards(transform.position, targetLocation, easing);
                if ((Vector2)transform.position == targetLocation)
                {
                    SetTargetLocation();
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
        print($"Moving to location: {targetLocation}");
    }
}
