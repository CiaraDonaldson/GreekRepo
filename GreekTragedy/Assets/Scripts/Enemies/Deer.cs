using System.Collections;
using UnityEngine;

public sealed class Deer : Enemy
{
    public enum DeerState
    {
        Moving,
        Attacking,
        Fleeing
    }

    public DeerState currentState;
    [Range(0f, .2f)] public float moveRate;
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
        switch (currentState)
        {
            case DeerState.Moving:
                transform.position = Vector2.MoveTowards((Vector2)transform.position, targetLocation, moveRate);
                if ((Vector2)transform.position == targetLocation)
                {
                    if (!isGettingNewTargetLocation)
                    {
                        isGettingNewTargetLocation = true;
                        StartCoroutine(DelayedTargetSet());
                    }
                }
                break;
            case DeerState.Attacking:
                break;
            case DeerState.Fleeing:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    void SetTargetLocation()
    {
        targetLocation = new Vector2(Random.Range(-PlayerMove.ROOM_SIZE.x * .5f, PlayerMove.ROOM_SIZE.x * .5f), Random.Range(-PlayerMove.ROOM_SIZE.y * .5f , PlayerMove.ROOM_SIZE.y * .5f));
    }
}
