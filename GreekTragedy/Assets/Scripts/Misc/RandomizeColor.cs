using UnityEngine;
[RequireComponent(typeof(Renderer))]
public sealed class RandomizeColor : MonoBehaviour
{
    [SerializeField] Color[] m_colors = new Color[0];
    private Renderer m_rend;

    private void Awake() => m_rend = GetComponent<Renderer>();

    private void OnEnable()
    {
        if (m_rend == null) return;
        if (m_colors.Length > 0)
        {
            foreach (var m in m_rend.materials)
                m.color = m_colors[Random.Range(0, m_colors.Length)];
        }
        else
        foreach (var m in m_rend.materials)
            m.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}
