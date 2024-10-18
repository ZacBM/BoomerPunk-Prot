using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private KeyCode endKey = KeyCode.R;

    void Update()
    {
        bool pressingStartKey = Input.GetKeyDown(endKey);
        if (pressingStartKey)
        {
            LoadFirstGameplayScene();
        }
    }

    void LoadFirstGameplayScene()
    {
        SceneManager.LoadScene(1);
    }
}