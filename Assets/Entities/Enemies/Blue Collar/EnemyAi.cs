using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAi : MonoBehaviour
{

    NavMeshAgent navMeshAgent;
    GameObject player;
    [SerializeField] float stoppingDistance;
    [SerializeField] float stayAwayDistance;
    public static List<EnemyAi> enemiesInAttackRange = new List<EnemyAi>();
    public static int maxAttackers = 4;

    float shuffleSpeed;
    float shuffleAmplitude;
    [SerializeField] float force;
    Rigidbody rb;
    [SerializeField] float timeToDie;

    [Header("Death Sounds")]
    public AudioClip[] deathSounds;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //rb.isKinematic = false;
        player = GameObject.FindWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        shuffleSpeed = Random.Range(1.0f, 3.0f);
        shuffleAmplitude = Random.Range(1.0f, 3.0f); ;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Chase();

    }

    void Chase()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);


        if (distanceToPlayer > stoppingDistance && enemiesInAttackRange.Count < maxAttackers)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }


        else if (distanceToPlayer <= stoppingDistance)
        {

            if (!enemiesInAttackRange.Contains(this) && enemiesInAttackRange.Count < maxAttackers)
            {
                enemiesInAttackRange.Add(this);
                //player.GetComponent<HPComponent>().ChangeHealth(-1);
                //deal damage on timer
            }
            else if(enemiesInAttackRange.Contains(this))
                navMeshAgent.SetDestination(transform.position);
        }


        else if (distanceToPlayer > stoppingDistance && enemiesInAttackRange.Count >= maxAttackers)
        {
            StayAway();

        }


        //if player moves out of range then remove from list
        if (enemiesInAttackRange.Contains(this) && distanceToPlayer > stoppingDistance)
        {
            enemiesInAttackRange.Remove(this);
        }
    }


    void StayAway()
    {

        Vector3 positionAwayFromPlayer = (transform.position - player.transform.position).normalized;
        Vector3 stayAwayPosition = player.transform.position + positionAwayFromPlayer * stayAwayDistance;

        Vector3 shuffleDirection = Vector3.Cross(positionAwayFromPlayer, Vector3.up).normalized;
        float shuffleOffset = Mathf.Sin(Time.time * shuffleSpeed) * shuffleAmplitude;

        Vector3 shufflePosition = stayAwayPosition + shuffleDirection * shuffleOffset;


        navMeshAgent.SetDestination(shufflePosition);
    }


    public void OnDeath()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        rb.isKinematic = false;

        Vector3 forceDirection = (transform.position - player.transform.position).normalized;

        rb.AddForce(forceDirection * force, ForceMode.Impulse);
        rb.AddForce(Vector3.up * force * 0.2f, ForceMode.Impulse);
        Invoke("DestroySelf", timeToDie);

    }

    public void PlayDeathSound()
    {
        if (deathSounds != null)
        {
            if (deathSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, deathSounds.Length);
                AudioSource.PlayClipAtPoint(deathSounds[randomIndex], transform.position);
                Debug.Log("Death Sound");
            }
        }
        
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (enemiesInAttackRange.Contains(this))
        {
            enemiesInAttackRange.Remove(this);
        }
    }

    void OnDisable()
    {
        GameManager.gameManager.numberOfEnemiesLeft--;
        if (enemiesInAttackRange.Contains(this))
        {
            enemiesInAttackRange.Remove(this);
        }
    }

}