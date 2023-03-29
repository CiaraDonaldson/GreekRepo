using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public bool showCursor;
    public void LoadLevel(int index) => SceneManager.LoadScene(index);

    public void LoadLevel(string sceneName) => SceneManager.LoadScene(sceneName);

    public void QuitGame() => Application.Quit();

    private void Start()
    {
        if (!showCursor)
            Cursor.visible = false;
    }
}
