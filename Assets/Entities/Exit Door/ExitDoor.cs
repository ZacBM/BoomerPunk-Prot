using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private float exitDistance;
    Transform player;

    void Update()
    {
        if (player == null) player = GameObject.Find("Player").transform;
        
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
                new Vector2(player.position.x, player.position.z)) <= exitDistance)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, exitDistance);
    }
}