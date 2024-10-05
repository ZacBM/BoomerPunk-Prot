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
    public static int maxAttackers = 6;

    float shuffleSpeed;
    float shuffleAmplitude;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        shuffleSpeed = Random.Range(1.0f, 3.0f);
        shuffleAmplitude = Random.Range(1.0f, 3.0f); ;


    }

    // Update is called once per frame
    void Update()
    {
        Chase();
    }

    void Chase()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);


        if (distanceToPlayer > stoppingDistance && enemiesInAttackRange.Count < maxAttackers) {
            navMeshAgent.SetDestination(player.transform.position);
        }


        else if(distanceToPlayer <= stoppingDistance)
        {

            if(!enemiesInAttackRange.Contains(this) && enemiesInAttackRange.Count < maxAttackers)
            {
                enemiesInAttackRange.Add(this);
                Debug.Log(enemiesInAttackRange);
            }
            navMeshAgent.SetDestination(transform.position);
        }


        else if (distanceToPlayer > stoppingDistance && distanceToPlayer < stayAwayDistance && enemiesInAttackRange.Count >= maxAttackers)
        {
            StayAway();
        }

        if (enemiesInAttackRange.Contains(this) && distanceToPlayer > stoppingDistance + 1.0f)
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


    void OnDestroy()
    {
        if (enemiesInAttackRange.Contains(this))
        {
            enemiesInAttackRange.Remove(this);
        }
    }

    void OnDisable()
    {
        if (enemiesInAttackRange.Contains(this))
        {
            enemiesInAttackRange.Remove(this);
        }
    }

}
