using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    /// <summary>  
    /// The exit door object.  
    ///  
    /// Spawns when a certain amount of enemies are killed  Transitions to an elevator scene.
    /// /// - Joshua  
    /// </summary>  
    
    [SerializeField] private float exitDistance;
    Transform player;

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        Vector2 playerPosition = new Vector2(player.position.x, player.position.z);
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), playerPosition) <= exitDistance)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, exitDistance);
    }
}