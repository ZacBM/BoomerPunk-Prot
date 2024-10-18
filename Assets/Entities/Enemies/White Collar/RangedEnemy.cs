using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.UI;
using UnityEngine.Serialization;
using static UnityEngine.GraphicsBuffer;

public class RangedEnemy : MonoBehaviour, Enemy
{
    [Header("Navigation")]
    NavigationComponent navComponent;
    
    [Header("Physics")]
    PhysicsComponent physicsComponent;
    
    public Transform projectileOriginTransform;
    public float explosionImpulse = 20f; // What is this? Could this be given a better name?

    [Header("Projectile Trajectory Rendering")]
    private LineRenderer lineRenderer;
    
    public int resolution = 50;
    public float tStep = 0.1f;

    [Header("Other")]
    [SerializeField] private float projectileSpawnDelaySet;
    private float projectileSpawnDelay;

    public GameObject cannonballPrefab;
    [SerializeField] private bool aimHigh = true;

    [SerializeField] float stayAwayDistance;
    [SerializeField] float tooCloseDistance;

    void Awake()
    {
        navComponent.target = GameObject.FindWithTag("Player").transform;
        
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        projectileSpawnDelay -= Time.deltaTime;
    }

    void FixedUpdate()
    {
       // DrawTrajectory();
       if (GetComponent<NavMeshAgent>().enabled)
       {
           Chase();
       }      
    }

    void Chase()
    {
        if (navComponent.DistanceToTarget() > stayAwayDistance)
        {
            navComponent.navAgent.SetDestination(navComponent.TargetPosition());
        }
        else if (navComponent.DistanceToTarget() <= tooCloseDistance)
        {
            Vector3 directionAwayFromPlayer = (transform.position - navComponent.TargetPosition()).normalized;
            Vector3 movePosition = navComponent.TargetPosition() + directionAwayFromPlayer * stayAwayDistance;
            navComponent.navAgent.SetDestination(movePosition);
        }

        if (navComponent.DistanceToTarget() <= tooCloseDistance && projectileSpawnDelay <= 0f)
        {
            aimHigh = false;
            Attack();
        }
        else if (navComponent.DistanceToTarget() <= stayAwayDistance + 1.5f && projectileSpawnDelay <= 0f)
        {
            navComponent.navAgent.SetDestination(transform.position);
            aimHigh = true;
            Attack();
        }
    }

    void Attack()
    {
        AimAtTarget();
        projectileSpawnDelay = projectileSpawnDelaySet;
        GameObject cannonball = Instantiate(cannonballPrefab, projectileOriginTransform.position, projectileOriginTransform.rotation);
        cannonball.GetComponent<Rigidbody>().velocity = projectileOriginTransform.forward * explosionImpulse;
    }

    void AimAtTarget()
    {
        Vector3 planeTarget = navComponent.TargetPosition();
        planeTarget.y = projectileOriginTransform.position.y;

        Vector3 direction = (planeTarget - projectileOriginTransform.position).normalized;

        transform.rotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, direction, Vector3.up), 0);

        GetAngleToTarget(planeTarget, navComponent.TargetPosition());
    }
    
    public void GetAngleToTarget(Vector3 planeTarget, Vector3 targetPos)
    {
        float v = explosionImpulse;
        float gravity = Mathf.Abs(Physics.gravity.y);

        float x = Vector3.Distance(projectileOriginTransform.position, planeTarget);

        float y = targetPos.y - projectileOriginTransform.position.y;

        float v2 = v * v;
        float v4 = v2 * v2;
        float x2 = x * x;

        float sq = (v4 - gravity * ((gravity * x2) + (2.0f * y * v2)));

        if (sq <= 0)
        {
            return;
        }

        if (aimHigh)
        {
            sq = Mathf.Sqrt(sq);
        }
        else
        {
            sq = -Mathf.Sqrt(sq);
        }

        float angle = Mathf.Atan2((v2 + sq), (gravity * x));

        Quaternion currentRotation = projectileOriginTransform.transform.rotation; 
        projectileOriginTransform.transform.rotation = Quaternion.Euler(Mathf.Rad2Deg * angle, currentRotation.eulerAngles.y, currentRotation.eulerAngles.z);
    }

    /* void DrawTrajectory()
    {
        Vector3 startPosition = spawnPoint.position;
        Vector3 velocity = spawnPoint.forward * explosionImpulse; 

        lineRenderer.positionCount = resolution;

        for (int i = 0; i < resolution; i++)
        {
            float t = i * tStep;
            Vector3 position = CalculatePositionAtTime(startPosition, velocity, t);
            lineRenderer.SetPosition(i, position);
        }
    }*/

    Vector3 CalculatePositionAtTime(Vector3 startPosition, Vector3 velocity, float time)
    {
        Vector3 position = startPosition + velocity * time + 0.5f * Physics.gravity * time * time;
        return position;
    }
}