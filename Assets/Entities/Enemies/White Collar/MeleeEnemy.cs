using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavigationComponent))]
[RequireComponent(typeof(PhysicsComponent))]

public class MeleeEnemy : Enemy
{
    /// <summary>
    /// The logic for the melee enemy type.
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
    
    public static List<Enemy> enemiesInAttackRange = new(); // I wonder if this can be moved to a more logical location.
    public static int maxAttackers = 4;

    private float _shuffleSpeed;
    private float _shuffleAmplitude;
    
    [SerializeField] private float secondsUntilDeath;

    protected override void Start()
    {
        base.Start();
        
        _shuffleSpeed = Random.Range(1.0f, 3.0f);
        _shuffleAmplitude = Random.Range(1.0f, 3.0f);
    }

    protected override void Chase()
    {
        if (_navComponent.DistanceToTarget() > _navComponent.navAgent.stoppingDistance && enemiesInAttackRange.Count < maxAttackers)
        {
            _navComponent.navAgent.SetDestination(_navComponent.target.transform.position);
            return;
        }
        
        if (_navComponent.DistanceToTarget() <= _navComponent.navAgent.stoppingDistance)
        {
            if (!enemiesInAttackRange.Contains(this) && enemiesInAttackRange.Count < maxAttackers)
            {
                enemiesInAttackRange.Add(this);
                //StartCoroutine(AttackPlayer());
            }
            else if (enemiesInAttackRange.Contains(this))
            {
                _navComponent.navAgent.SetDestination(transform.position);
            }
        }
        else if (_navComponent.DistanceToTarget() > _navComponent.navAgent.stoppingDistance && enemiesInAttackRange.Count >= maxAttackers)
        {
            StayAway();
        }

        //if player moves out of range then remove from list
        if (enemiesInAttackRange.Contains(this) && _navComponent.DistanceToTarget() > _navComponent.navAgent.stoppingDistance)
        {
            StopAllCoroutines();
            enemiesInAttackRange.Remove(this);
        }
    }

    void StayAway()
    {
        Vector3 positionAwayFromPlayer = (transform.position - _navComponent.target.transform.position).normalized;
        Vector3 stayAwayPosition = _navComponent.target.transform.position + positionAwayFromPlayer * stayAwayDistance;

        Vector3 shuffleDirection = Vector3.Cross(positionAwayFromPlayer, Vector3.up).normalized;
        float shuffleOffset = Mathf.Sin(Time.time * _shuffleSpeed) * _shuffleAmplitude;

        Vector3 shufflePosition = stayAwayPosition + shuffleDirection * shuffleOffset;

        _navComponent.navAgent.SetDestination(shufflePosition);
    }

    public void OnDeath()
    {
        _physicsComponent.Knockback(10f, 2f, _navComponent.target.transform.position);
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
            _physicsComponent.Knockback(5f, 4f, _navComponent.target.transform.position);
        }
        if (otherCollider.gameObject.tag == "Thrown Weapon")
        {
            _physicsComponent.Knockback(5f, 4f, _navComponent.target.transform.position);
        }
    }
}