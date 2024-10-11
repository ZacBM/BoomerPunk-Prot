using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Allows for Singleton.
    public static GameManager gameManager;
    
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerCameraHolderPrefab;
    
    [SerializeField] private GameObject exitDoorPrefab;
    [SerializeField] private Vector3 exitDoorLocation;
    private int currentScene;
    
    private GameObject[] enemies;
    [SerializeField] private float percentOfEnemiesToLeaveAliveToProgress;
    [SerializeField] private int numberOfEnemiesToLeaveAliveToProgress;
    public int numberOfEnemiesLeft;
    public bool canProgressToNextFloor = false;
    
    void Awake()
    {
        if (gameManager != null && gameManager != this) Destroy(gameObject);
        else gameManager = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;

        //StartCoroutine(DetermineAmountOfEnemiesToLeaveAlive());
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        numberOfEnemiesLeft = enemies.Length;
        numberOfEnemiesToLeaveAliveToProgress = Mathf.CeilToInt((float)numberOfEnemiesLeft * (percentOfEnemiesToLeaveAliveToProgress / 100f));
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

        if (!canProgressToNextFloor && numberOfEnemiesLeft <= numberOfEnemiesToLeaveAliveToProgress)
        {
            canProgressToNextFloor = true;
            Instantiate(exitDoorPrefab, exitDoorLocation, Quaternion.identity);
        }
    }
    
    IEnumerator DetermineAmountOfEnemiesToLeaveAlive()
    {
        yield return new WaitForSeconds(0.1f);
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        numberOfEnemiesLeft = enemies.Length;
        yield return new WaitForSeconds(1f);
        numberOfEnemiesToLeaveAliveToProgress = Mathf.CeilToInt((float)numberOfEnemiesLeft * (percentOfEnemiesToLeaveAliveToProgress / 100f));
    }
}