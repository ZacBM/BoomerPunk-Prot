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

    [Header("Enemy Tracking")]
    [SerializeField] private float percentageOfEnemiesToKillToProgress;

    private int numberOfEnemiesToKillToProgress;
    private int startingEnemyCount;
    [HideInInspector] public bool canProgressToNextFloor = false;
    [HideInInspector] public int numberOfEnemiesKilled;
    private bool dingPlayed = false;

    [Header("Audio")] [SerializeField] private AudioSource elevatorDing;

    void Awake()
    {
        if (gameManager != null && gameManager != this) Destroy(gameObject);
        else gameManager = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    void Update()
    {
        // This will receive fixes & refinements later.
        /*if (currentScene != SceneManager.GetActiveScene().buildIndex)
        {
            currentScene = SceneManager.GetActiveScene().buildIndex;
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            Instantiate(playerCameraHolderPrefab, Vector3.zero, Quaternion.identity);
        }*/

        if (!canProgressToNextFloor && startingEnemyCount > 0 &&
            numberOfEnemiesKilled >= numberOfEnemiesToKillToProgress)
        {
            canProgressToNextFloor = true;
            GameObject exitDoor = Instantiate(exitDoorPrefab, exitDoorLocation, Quaternion.identity);
            elevatorDing = exitDoor.GetComponent<AudioSource>();
        }

        if (canProgressToNextFloor && elevatorDing != null && !dingPlayed)
        {
            dingPlayed = true;
            StartCoroutine(PlayDing());
        }
    }

    IEnumerator PlayDing()
    {
        yield return new WaitForSeconds(1.5f);
        if (elevatorDing.enabled)
        {
            elevatorDing.Play();
            Debug.Log("DING!");
        }
    }

    private void ActiveSceneChanged(Scene current, Scene next)
    {
        GetEnemyStatistics();
    }

    public void GetEnemyStatistics()
    {
        canProgressToNextFloor = false;
        numberOfEnemiesKilled = 0;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        startingEnemyCount = enemies.Length;
        numberOfEnemiesToKillToProgress =
            Mathf.FloorToInt(startingEnemyCount * (percentageOfEnemiesToKillToProgress / 100f));
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GetEnemyStatistics();
    }
}