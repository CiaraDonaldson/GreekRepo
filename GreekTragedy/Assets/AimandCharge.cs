using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimandCharge : MonoBehaviour
{
    public float Range;

    public Transform Target;

    bool Detected = false;

    Vector2 Direction;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetPos = Target.position;

        Direction = targetPos - (Vector2)transform.position;
        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Range);

        if (rayInfo)
        {
            if (rayInfo.collider.gameObject.tag == "Player")
            {
                if (Detected == false)
                {
                    Detected = true;
                    this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                }
                else
                {
                    if (Detected == true)
                    {
                        Detected = false;
                        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
        }
        if (Detected)
        {
            this.gameObject.transform.right = Direction;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
