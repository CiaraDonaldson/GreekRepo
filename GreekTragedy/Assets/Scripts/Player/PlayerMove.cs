using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static Vector2 ROOM_CENTER, ROOM_SIZE;
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 roomCenter;
    [SerializeField] Vector2 roomSize;
    [SerializeField] Vector2 playerOffset;
    Vector2 _moveDirection;

    void Update()
    {
        _moveDirection.x = Input.GetAxis("Horizontal");
        _moveDirection.y = Input.GetAxis("Vertical");
        Vector2 newPos = (Vector2)transform.position + moveSpeed * Time.deltaTime * _moveDirection;
        Vector2 roomMin = roomCenter - roomSize / 2;
        Vector2 roomMax = roomCenter + roomSize / 2;
        newPos.x = Mathf.Clamp(newPos.x, roomMin.x + playerOffset.x, roomMax.x - playerOffset.x);
        newPos.y = Mathf.Clamp(newPos.y, roomMin.y + playerOffset.y, roomMax.y - playerOffset.y);
        transform.position = newPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(roomCenter, roomSize);
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(roomCenter, roomSize - playerOffset);
    }

    private void OnValidate()
    {
        ROOM_SIZE = roomSize;
        ROOM_CENTER = roomCenter;
    }
}
