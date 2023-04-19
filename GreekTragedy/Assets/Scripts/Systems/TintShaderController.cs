using System.Collections;
using UnityEngine;

public class TintShaderController : MonoBehaviour
{
    public Shader customSpriteShader;
    public float tintDuration = 1f; // Duration for tint changes
    private Material spriteMaterial;
    private Coroutine tintCoroutine;

    private void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        spriteMaterial = meshRenderer.material;
    }

    public void SetTintR(float value)
    {
        if (tintCoroutine != null)
        {
            StopCoroutine(tintCoroutine);
        }
        tintCoroutine = StartCoroutine(TintRCoroutine(value));
    }

    public void SetTintRImmediate(float value) => spriteMaterial.SetFloat("_TintR", value);

    public void SetTintGImmediate(float value) => spriteMaterial.SetFloat("_TintG", value);

    public void SetTintBImmediate(float value) => spriteMaterial.SetFloat("_TintB", value);

    public void SetTintG(float value)
    {
        if (tintCoroutine != null)
        {
            StopCoroutine(tintCoroutine);
        }
        tintCoroutine = StartCoroutine(TintGCoroutine(value));
    }

    public void SetTintB(float value)
    {
        if (tintCoroutine != null)
        {
            StopCoroutine(tintCoroutine);
        }
        tintCoroutine = StartCoroutine(TintBCoroutine(value));
    }

    private IEnumerator TintRCoroutine(float value)
    {
        float currentTintR = spriteMaterial.GetFloat("_TintR");
        float elapsedTime = 0f;
        while (elapsedTime < tintDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / tintDuration);
            spriteMaterial.SetFloat("_TintR", Mathf.Lerp(currentTintR, value, t));
            yield return null;
        }
        spriteMaterial.SetFloat("_TintR", value);
    }

    private IEnumerator TintGCoroutine(float value)
    {
        float currentTintG = spriteMaterial.GetFloat("_TintG");
        float elapsedTime = 0f;
        while (elapsedTime < tintDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / tintDuration);
            spriteMaterial.SetFloat("_TintG", Mathf.Lerp(currentTintG, value, t));
            yield return null;
        }
        spriteMaterial.SetFloat("_TintG", value);
    }

    private IEnumerator TintBCoroutine(float value)
    {
        float currentTintB = spriteMaterial.GetFloat("_TintB");
        float elapsedTime = 0f;
        while (elapsedTime < tintDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / tintDuration);
            spriteMaterial.SetFloat("_TintB", Mathf.Lerp(currentTintB, value, t));
            yield return null;
        }
        spriteMaterial.SetFloat("_TintB", value);
    }

    private void UpdateSpriteMaterial()
    {
        if (spriteMaterial.shader == customSpriteShader) return;
        spriteMaterial = new Material(customSpriteShader);
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = spriteMaterial;
    }

    private void OnRenderObject()
    {
        if (spriteMaterial != null)
            spriteMaterial.SetPass(0);
    }
}

