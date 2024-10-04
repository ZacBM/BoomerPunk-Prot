using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Allows for Singleton.
    public static GameManager gameManager;
    
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerCameraHolderPrefab;
    
    int currentScene;
    
    void Awake()
    {
        if (gameManager != null && gameManager != this) Destroy(gameObject);
        else gameManager = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        // This will receive fixes & refinements later.
        if (currentScene != SceneManager.GetActiveScene().buildIndex)
        {
            currentScene = SceneManager.GetActiveScene().buildIndex;
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            Instantiate(playerCameraHolderPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
