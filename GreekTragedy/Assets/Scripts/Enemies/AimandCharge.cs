using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimandCharge : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float launchSpeed = 10f;
    

    private Transform playerTransform;
    private Vector3 launchTarget;
    public bool launching = false;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (launching == false)
        {
            StartCoroutine(LaunchAfterWait());
            // Rotate towards player's position
            Vector3 targetDirection = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            // Save player's last position for launch
            launchTarget = playerTransform.position;
            
        }
        else if(launching == true)
        {
            // Launch towards player's last position
            transform.position = Vector3.MoveTowards(transform.position, launchTarget, launchSpeed * Time.deltaTime);
            // Destroy once reaching launch target
            if (transform.position == launchTarget)
            {
                Destroy(gameObject);
            }
        }

    }

    IEnumerator LaunchAfterWait()
    {
        yield return new WaitForSeconds(3f);
        launching = true;

    }
}
