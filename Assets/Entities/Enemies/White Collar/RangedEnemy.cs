using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.UI;
using UnityEngine.Serialization;
using static UnityEngine.GraphicsBuffer;

public class RangedEnemy : Enemy
{
    /// <summary>
    /// The logic for the ranged enemy type.
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
    ///
    /// - Joshua
    /// </remarks>
    
    public Transform projectileOriginTransform;
    public float explosionImpulse = 20f; // What is this? Could this be given a better name?

    [Header("Projectile Trajectory Rendering")]
    private LineRenderer _lineRenderer;
    
    [SerializeField] private int resolution = 50;
    [SerializeField] private float tStep = 0.1f;

    [Header("Other")]
    [SerializeField] private float projectileSpawnDelaySet;
    private float _projectileSpawnDelay;

    public GameObject cannonballPrefab;
    [SerializeField] private bool aimHigh = true;

    [SerializeField] private float tooCloseDistance;

    protected override void Awake()
    {
        base.Awake();
        
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        _projectileSpawnDelay -= Time.deltaTime;
    }

    protected override void Chase()
    {
        if (_navComponent.DistanceToTarget() > stayAwayDistance)
        {
            _navComponent.navAgent.SetDestination(_navComponent.TargetPosition());
        }
        else if (_navComponent.DistanceToTarget() <= tooCloseDistance)
        {
            Vector3 directionAwayFromPlayer = (transform.position - _navComponent.TargetPosition()).normalized;
            Vector3 movePosition = _navComponent.TargetPosition() + directionAwayFromPlayer * stayAwayDistance;
            _navComponent.navAgent.SetDestination(movePosition);
        }

        if (_navComponent.DistanceToTarget() <= tooCloseDistance && _projectileSpawnDelay <= 0f)
        {
            aimHigh = false;
            Attack();
        }
        else if (_navComponent.DistanceToTarget() <= stayAwayDistance + 1.5f && _projectileSpawnDelay <= 0f)
        {
            _navComponent.navAgent.SetDestination(transform.position);
            aimHigh = true;
            Attack();
        }
    }

    void Attack()
    {
        AimAtTarget();
        _projectileSpawnDelay = projectileSpawnDelaySet;
        GameObject cannonball = Instantiate(cannonballPrefab, projectileOriginTransform.position, projectileOriginTransform.rotation);
        cannonball.GetComponent<Rigidbody>().velocity = projectileOriginTransform.forward * explosionImpulse;
    }

    void AimAtTarget()
    {
        Vector3 planeTarget = _navComponent.TargetPosition();
        planeTarget.y = projectileOriginTransform.position.y;

        Vector3 direction = (planeTarget - projectileOriginTransform.position).normalized;

        transform.rotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, direction, Vector3.up), 0);

        GetAngleToTarget(planeTarget, _navComponent.TargetPosition());
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