using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Allows for Singleton.
    public static GameManager gameManager;
    
    void Awake()
    {
        if (gameManager != null && gameManager != this) Destroy(this.gameObject);
        else gameManager = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
