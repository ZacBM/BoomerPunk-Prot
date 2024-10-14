using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAi : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    GameObject player;
    [SerializeField] float stoppingDistance;
    [SerializeField] float tooCloseDistance;
    [SerializeField] float stayAwayDistance;
    public static List<EnemyAi> enemiesInAttackRange = new();
    public static int maxAttackers = 4;

    float shuffleSpeed;
    float shuffleAmplitude;
    [SerializeField] float force;
    Rigidbody rb;
    [SerializeField] float timeToDie;

    [SerializeField] int damage;
    [SerializeField] float timeBetweenAttacks;
    

    private void Awake()
    {
        NavMeshSurface surface = FindObjectOfType<NavMeshSurface>();
        if (surface == null)
        {
            CreateAndBakeNamMeshSurface();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        if(player == null)
            Destroy(gameObject);
        navMeshAgent = GetComponent<NavMeshAgent>();
        shuffleSpeed = Random.Range(1.0f, 3.0f);
        shuffleAmplitude = Random.Range(1.0f, 3.0f);
    }

    void FixedUpdate()
    {
        if (navMeshAgent != null && navMeshAgent.enabled)
        {
            Chase();
        }
    }
    
    void CreateAndBakeNamMeshSurface()
    {
        NavMeshSurface surface = FindObjectOfType<NavMeshSurface>();
        if (surface != null)
        {
            return;
        }
        GameObject newNavMeshSurface = new GameObject();
        newNavMeshSurface.name = "NavMesh Surface";
        newNavMeshSurface.AddComponent<NavMeshSurface>();
        newNavMeshSurface.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    void Chase()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > stoppingDistance && enemiesInAttackRange.Count < maxAttackers)
        {
            if (navMeshAgent != null)
            {
                navMeshAgent.SetDestination(player.transform.position);
                return;
            } 
        }

        else if (distanceToPlayer <= stoppingDistance)
        {
            if (!enemiesInAttackRange.Contains(this) && enemiesInAttackRange.Count < maxAttackers)
            {
                enemiesInAttackRange.Add(this);
                StartCoroutine(AttackPlayer());
            }
            else if (enemiesInAttackRange.Contains(this))
            {
                navMeshAgent.SetDestination(transform.position);
            }
        }

        else if (distanceToPlayer > stoppingDistance && enemiesInAttackRange.Count >= maxAttackers)
        {
            StayAway();
        }

        //if player moves out of range then remove from list
        if (enemiesInAttackRange.Contains(this) && distanceToPlayer > stoppingDistance)
        {
            StopAllCoroutines();
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
    
    public void Knockback()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        rb.isKinematic = false;

        Vector3 forceDirection = (transform.position - player.transform.position).normalized;

        rb.AddForce(forceDirection * force, ForceMode.Impulse);
        rb.AddForce(Vector3.up * force * 0.2f, ForceMode.Impulse);
    }
    
    //copy and paste of knockback function, they can be the same
    public void SmallKnockback()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        rb.isKinematic = false;

        Vector3 forceDirection = (transform.position - player.transform.position).normalized;

        rb.AddForce(forceDirection * (force / 4f), ForceMode.Impulse);
        //rb.AddForce(Vector3.up * force * 0.2f, ForceMode.Impulse);
    }

    public void OnDeath()
    {
        Knockback();
        Invoke("DestroySelf", timeToDie);
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
        if(GameManager.gameManager != null)
            GameManager.gameManager.numberOfEnemiesLeft--;
        if (enemiesInAttackRange.Contains(this))
        {
            enemiesInAttackRange.Remove(this);
        }
    }

    void TrackAgain()
    {
        GetComponent<NavMeshAgent>().enabled = true;
        rb.isKinematic = true;
    }

    //this doesnt do anything
    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Melee Weapon")
        {
            SmallKnockback();
            Invoke("TrackAgain", (timeToDie / 2f));
        }
        if (otherCollider.gameObject.tag == "Thrown Weapon")
        {
            SmallKnockback();
            Invoke("TrackAgain", (timeToDie / 2f));
        }
    }


    IEnumerator AttackPlayer()
    {
        while (enemiesInAttackRange.Contains(this)) {
            player.GetComponent<HPComponent>().ChangeHealth(-damage);
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }
}