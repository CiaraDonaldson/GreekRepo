using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(int index) => SceneManager.LoadScene(index);

    public void LoadLevel(string sceneName) => SceneManager.LoadScene(sceneName);
}
