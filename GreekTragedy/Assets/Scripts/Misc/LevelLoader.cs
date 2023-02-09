using UnityEngine;

public sealed class LevelLoader : MonoBehaviour
{
    public void LoadLevel(int indx) => UnityEngine.SceneManagement.SceneManager.LoadScene(indx);
}
