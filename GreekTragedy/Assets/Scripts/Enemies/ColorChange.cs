using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    public Renderer bodyRenderer;
    public float speed;
    public Color startColor, endColor;
    float startTime;

    public GameObject effect;

    public float radius = 50f;
    private Vector3 center;
    public int attackDamage;

    void Start()
    {
        StartCoroutine(ChangeEngineColour());
      
        
    }
    void Update()
    {
        if (bodyRenderer.material.color == endColor)
        {
            effect.SetActive(true);
            StartCoroutine(WaitTime());

            this.gameObject.TryGetComponent(out IDamagable success);                
                success.ApplyDamage(this.gameObject, attackDamage); 
                

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);

            foreach (Collider2D nearbyObject in hitColliders)
            {
                Debug.Log("Something here");
                if (nearbyObject.gameObject.tag == "Player")
                {
                    if (nearbyObject.gameObject.TryGetComponent(out IDamagable damage))
                    {
                        damage.ApplyDamage(nearbyObject.gameObject, attackDamage);
                        if (nearbyObject.TryGetComponent(out PlayerMove playerMove))
                            playerMove.KnockBack(nearbyObject.transform.position - transform.position);
                       
                    }
                    Debug.Log("Hit");
                }
            }
        }
    }
    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1f);

    }

    private IEnumerator ChangeEngineColour()
    {
        float tick = 0f;
        while (bodyRenderer.material.color != endColor)
        {
            tick += Time.deltaTime * speed;
            bodyRenderer.material.color = Color.Lerp(startColor, endColor, tick);
            yield return null;
        }
    }
    private void OnDrawGizmosSelected() // for visualizing location in scene vew during play
    { 
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }
}

