using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private KeyCode startKey = KeyCode.Space;

    void Update()
    {
        if (Input.GetKeyDown(startKey)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
