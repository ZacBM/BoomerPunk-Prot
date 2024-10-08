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
        healthChange.AddListener(enemyAi.OnDeath);
    }

    public int ChangeHealth(int changeInHealth)
    {
        health += changeInHealth;
        if (health <= 0) Die();
        if (health > maxHealth) health = maxHealth;
        return health;
    }

    void Die()
    {
        if (enemyAi != null)
        {
            enemyAi.PlayDeathSound();
            healthChange.Invoke();
        }
        if (behaviorOnDeath == BehaviorsOnDeath.DEACTIVATE) gameObject.SetActive(false);
        else if (behaviorOnDeath == BehaviorsOnDeath.DELETE) Destroy(gameObject);
    }
}
