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
    NavigationComponent navComponent;
    
    [Header("Physics")]
    PhysicsComponent physicsComponent;
    
    [SerializeField] private float stayAwayDistance;
    public static List<Enemy> enemiesInAttackRange = new();
    public static int maxAttackers = 4;

    private float shuffleSpeed;
    private float shuffleAmplitude;
    
    [SerializeField] private float secondsUntilDeath;

    void Start()
    {
        navComponent.target = GameObject.FindWithTag("Player").transform;

        shuffleSpeed = Random.Range(1.0f, 3.0f);
        shuffleAmplitude = Random.Range(1.0f, 3.0f);
    }

    void FixedUpdate()
    {
        Chase();
    }

    void Chase()
    {
        if (navComponent.DistanceToTarget() > navComponent.navAgent.stoppingDistance && enemiesInAttackRange.Count < maxAttackers)
        {
            navComponent.navAgent.SetDestination(navComponent.target.transform.position);
            return;
        }
        
        if (navComponent.DistanceToTarget() <= navComponent.navAgent.stoppingDistance)
        {
            if (!enemiesInAttackRange.Contains(this) && enemiesInAttackRange.Count < maxAttackers)
            {
                enemiesInAttackRange.Add(this);
                //StartCoroutine(AttackPlayer());
            }
            else if (enemiesInAttackRange.Contains(this))
            {
                navComponent.navAgent.SetDestination(transform.position);
            }
        }
        else if (navComponent.DistanceToTarget() > navComponent.navAgent.stoppingDistance && enemiesInAttackRange.Count >= maxAttackers)
        {
            StayAway();
        }

        //if player moves out of range then remove from list
        if (enemiesInAttackRange.Contains(this) && navComponent.DistanceToTarget() > navComponent.navAgent.stoppingDistance)
        {
            StopAllCoroutines();
            enemiesInAttackRange.Remove(this);
        }
    }

    void StayAway()
    {
        Vector3 positionAwayFromPlayer = (transform.position - navComponent.target.transform.position).normalized;
        Vector3 stayAwayPosition = navComponent.target.transform.position + positionAwayFromPlayer * stayAwayDistance;

        Vector3 shuffleDirection = Vector3.Cross(positionAwayFromPlayer, Vector3.up).normalized;
        float shuffleOffset = Mathf.Sin(Time.time * shuffleSpeed) * shuffleAmplitude;

        Vector3 shufflePosition = stayAwayPosition + shuffleDirection * shuffleOffset;

        navComponent.navAgent.SetDestination(shufflePosition);
    }

    public void OnDeath()
    {
        physicsComponent.Knockback(10f, 2f, navComponent.target.transform.position);
        Invoke("DestroySelf", secondsUntilDeath);
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
        if (GameManager.gameManager != null)
        {
            GameManager.gameManager.numberOfEnemiesLeft--;
        }
        if (enemiesInAttackRange.Contains(this))
        {
            enemiesInAttackRange.Remove(this);
        }
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Melee Weapon")
        {
            physicsComponent.Knockback(5f, 4f, navComponent.target.transform.position);
        }
        if (otherCollider.gameObject.tag == "Thrown Weapon")
        {
            physicsComponent.Knockback(5f, 4f, navComponent.target.transform.position);
        }
    }
}