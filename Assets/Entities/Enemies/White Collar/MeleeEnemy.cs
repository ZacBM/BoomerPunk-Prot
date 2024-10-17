using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MeleeEnemy : MonoBehaviour, Enemy
{
    [Header("Navigation")]
    NavigationComponent navigationComponent;
    NavMeshAgent navMeshAgent;
    
    [Header("Physics")]
    PhysicsComponent physicsComponent;
    Rigidbody rigidbody;
    
    GameObject player;
    
    //[SerializeField] float stayAwayDistance;
    public static List<Enemy> enemiesInAttackRange = new();
    public static int maxAttackers = 4;

    float shuffleSpeed;
    float shuffleAmplitude;
    [SerializeField] float force;
    
    [SerializeField] float timeToDie;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        navigationComponent.target = GameObject.FindWithTag("Player").transform;
        if (player == null)
        {
            Destroy(gameObject);
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        shuffleSpeed = Random.Range(1.0f, 3.0f);
        shuffleAmplitude = Random.Range(1.0f, 3.0f);
    }

    void FixedUpdate()
    {
        if (navMeshAgent.enabled)
        {
            Chase();
        }
    }

    void Chase()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > navMeshAgent.stoppingDistance && enemiesInAttackRange.Count < maxAttackers)
        {
            if (navMeshAgent != null)
            {
                navMeshAgent.SetDestination(player.transform.position);
                return;
            }
        }
        else if (distanceToPlayer <= navMeshAgent.stoppingDistance)
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
        else if (distanceToPlayer > navMeshAgent.stoppingDistance && enemiesInAttackRange.Count >= maxAttackers)
        {
            StayAway();
        }

        //if player moves out of range then remove from list
        if (enemiesInAttackRange.Contains(this) && distanceToPlayer > navMeshAgent.stoppingDistance)
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

    public void OnDeath()
    {
        physicsComponent.Knockback(10f, 2f, player.transform.position);
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
}