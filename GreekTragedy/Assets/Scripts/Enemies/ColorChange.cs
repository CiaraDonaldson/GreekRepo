using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    public Renderer engineBodyRenderer;
    public float speed;
    public Color startColor, endColor;
    float startTime;


    public float radius = 50f;
    private Vector3 center;
    public int attackDamage;

    void Start()
    {
        StartCoroutine(ChangeEngineColour());
      
        
    }
    void Update()
    {
        if (engineBodyRenderer.material.color == endColor)
        {

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

    private IEnumerator ChangeEngineColour()
    {
        float tick = 0f;
        while (engineBodyRenderer.material.color != endColor)
        {
            tick += Time.deltaTime * speed;
            engineBodyRenderer.material.color = Color.Lerp(startColor, endColor, tick);
            yield return null;
        }
    }
    private void OnDrawGizmosSelected() // for visualizing location in scene vew during play
    { 
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }
}

