using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavigationComponent))]
[RequireComponent(typeof(PhysicsComponent))]

public abstract class Enemy : MonoBehaviour
{
    [Header("Navigation")]
    protected NavigationComponent navComponent;
    
    [Header("Physics")]
    protected PhysicsComponent physicsComponent;
    
    [SerializeField] protected float stayAwayDistance;
    
    protected virtual void Start()
    {
        navComponent = GetComponent<NavigationComponent>();
        navComponent.target = GameObject.FindWithTag("Player").transform;
        
        physicsComponent = GetComponent<PhysicsComponent>();
    }
    
    protected virtual void FixedUpdate()
    {
        if (GetComponent<NavMeshAgent>().enabled)
        {
            Chase();
        }      
    }

    protected abstract void Chase();
}