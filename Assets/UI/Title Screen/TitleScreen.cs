using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private KeyCode startKey = KeyCode.Space;

    void Update()
    {
        bool pressingStartKey = Input.GetKeyDown(startKey);
        if (pressingStartKey)
        {
            LoadNextSceneInBuildSettings();
        }
    }

    public void SwapScene()
    {
        LoadNextSceneInBuildSettings();
    }

    void LoadNextSceneInBuildSettings()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
