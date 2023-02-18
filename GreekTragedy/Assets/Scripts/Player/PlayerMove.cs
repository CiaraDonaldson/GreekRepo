using darcproducts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public static Vector2 ROOM_CENTER, ROOM_SIZE;
    [SerializeField] float moveSpeed;
    [SerializeField] KeyCode dashKey;
    [SerializeField] float dashDistance;
    [SerializeField] float dashResetTime;
    float _currentTime;
    [SerializeField] Vector2 roomCenter;
    [SerializeField] Vector2 roomSize;
    [SerializeField] Vector2 playerOffset;
    [SerializeField] Image dashIndicator;
    [SerializeField] UnityEvent<Vector3> OnDashActivated;
    Vector2 _moveDirection;


    void Update()
    {
        _currentTime = _currentTime > dashResetTime ? dashResetTime : _currentTime += Time.deltaTime;
        _moveDirection.x = Input.GetAxis("Horizontal");
        _moveDirection.y = Input.GetAxis("Vertical");
        dashIndicator.fillAmount = Utilities.Remap(_currentTime, 0, dashResetTime, 0, 1);
        Vector2 newPos = (Vector2)transform.position + moveSpeed * Time.deltaTime * _moveDirection;
        Vector2 roomMin = roomCenter - roomSize / 2;
        Vector2 roomMax = roomCenter + roomSize / 2;
        newPos.x = Mathf.Clamp(newPos.x, roomMin.x + playerOffset.x, roomMax.x - playerOffset.x);
        newPos.y = Mathf.Clamp(newPos.y, roomMin.y + playerOffset.y, roomMax.y - playerOffset.y);
        if (Input.GetKey(dashKey) && _currentTime == dashResetTime)
        {
            _currentTime = 0;
            newPos += _moveDirection * dashDistance;
            Vector3 halfPos = (transform.position + (Vector3)newPos) * .5f;
            OnDashActivated?.Invoke(halfPos);
        }
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
