using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    /// <summary>
    /// The exit door object.
    ///
    /// Spawns when a certain amount of enemies
    /// 
    /// - Joshua
    /// </summary>
    
    /// <remarks>
    /// I would prefer that the Enemy base class be completely replace by components & unique state behaviors, but for
    /// the moment, the abstract base class Enemy has been created to make the codebase D.R.Y.er.
    ///
    /// Changes made after Prototype 2:
    /// - Some functions & variables moved to NavigationComponent & PhysicsComponent
    /// - Some variables renamed to improved clarity
    ///
    /// Changes to make:
    /// - State pattern implementation
    /// - Increased composition
    ///
    /// - Joshua
    /// </remarks>
    
    [SerializeField] private float exitDistance;
    private Transform _player;

    void Update()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        Vector2 playerPosition = new Vector2(_player.position.x, _player.position.z);
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