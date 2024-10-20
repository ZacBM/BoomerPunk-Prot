using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavigationComponent))]
[RequireComponent(typeof(PhysicsComponent))]

public abstract class Enemy : MonoBehaviour
{
    [Header("Navigation")]
    protected NavigationComponent _navComponent;
    
    [Header("Physics")]
    protected PhysicsComponent _physicsComponent;
    
    [SerializeField] protected float stayAwayDistance;
    
    protected virtual void Awake()
    {
        _navComponent = GetComponent<NavigationComponent>();
        
        _physicsComponent = GetComponent<PhysicsComponent>();
    }
    
    protected virtual void Start()
    {
        _navComponent.target = GameObject.FindWithTag("Player").transform;
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