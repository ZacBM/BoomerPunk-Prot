using UnityEngine;
using UnityEngine.Events;

public class HPComponent : MonoBehaviour
{
    [Range(0, 200)]
    public int health = 100;
    [Range(0, 200)]
    public int maxHealth = 100;
    
    [SerializeField] enum BehaviorsOnDeath {DEACTIVATE, DELETE, NOTHING}
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
