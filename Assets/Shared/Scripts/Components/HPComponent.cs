using UnityEngine;
using UnityEngine.Events;

public class HPComponent : MonoBehaviour
{
    /// <summary>  
    /// A component that manages the vitality of the object that possess it.
    ///
    /// The HPComponent acts as the health of the object. If health reaches zero, an appropriate & specified action
    /// takes place.
    /// /// - Joshua  
    /// </summary>  
  
    /// <remarks>  
    /// Changes to make:
    /// - Remove the EnemyAi field
    /// - - As it stands, the component references & mutates the user, which is fundamentally incorrect, and makes both
    /// - - parties tougher to reason about & debug
    /// - Make "health" read only to improve reasonability
    ///  
    /// - Joshua  
    /// </remarks>
    
    [Range(0, 200)]
    public int health = 100;
    [Range(0, 200)]
    public int maxHealth = 100;
    
    enum BehaviorsOnDeath {DEACTIVATE, DELETE, NOTHING}
    [SerializeField] BehaviorsOnDeath behaviorOnDeath = BehaviorsOnDeath.DELETE;

    private EnemyAi enemyAi;

    public UnityEvent healthChange;

    private void Awake()
    {
        enemyAi = GetComponent<EnemyAi>();
        if (enemyAi != null)
        {
            healthChange.AddListener(enemyAi.OnDeath);
        }
    }

    public int ChangeHealth(int changeInHealth)
    {
        health += changeInHealth;
        if (IsDead())
        {
            Die();
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        return health;
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    void Die()
    {
        try
        {
            gameObject.GetComponent<OnDeath>().Die();
        }
        catch
        {
            if (behaviorOnDeath == BehaviorsOnDeath.DELETE)
                Destroy(gameObject);
            else 
                gameObject.SetActive(false);
        }



        /*if (enemyAi != null)
        {
            enemyAi.PlayDeathSound();
            healthChange.Invoke();

        }*/
        

    }
}
