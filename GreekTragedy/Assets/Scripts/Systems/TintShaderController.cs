using UnityEngine;

public class TintShaderController : MonoBehaviour
{
    public Shader customSpriteShader;
    private Material spriteMaterial;

    private void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        spriteMaterial = meshRenderer.material;
    }

    public void SetTintR(float value)
    {
        UpdateSpriteMaterial();
        spriteMaterial.SetFloat("_TintR", value);
    }

    public void SetTintG(float value)
    {
        UpdateSpriteMaterial();
        spriteMaterial.SetFloat("_TintG", value);
    }

    public void SetTintB(float value)
    {
        UpdateSpriteMaterial();
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
