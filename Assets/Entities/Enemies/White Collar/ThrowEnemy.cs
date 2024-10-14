using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.UI;
using static UnityEngine.GraphicsBuffer;

public class ThrowEnemy : MonoBehaviour
{

    public Transform spawnPoint;
    public float explosionImpulse = 20f;
    public int resolution = 50;
    public float tStep = 0.1f;

    private LineRenderer lineRenderer;
    private Vector3 gravity;

    [SerializeField] float maxTimeBetweenAttacks;
    float timer;

    public GameObject cannonballPrefab;

    private float targetRotationY;

    public bool aimHigh = true;

    private float targetRotationCannonX;

    [SerializeField] float stayAwayDistance;
    [SerializeField] float tooCloseDistance;
    NavMeshAgent navMeshAgent;
    GameObject player;


    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
        gravity = Physics.gravity;
        player = GameObject.FindWithTag("Player");
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        DrawTrajectory();
        if (navMeshAgent != null && GetComponent<NavMeshAgent>().enabled == true)
            Chase();      
    }

    void Chase()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if(distanceToPlayer > stayAwayDistance)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }

        else if (distanceToPlayer <= tooCloseDistance)
        {
            Vector3 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;
            Vector3 movePosition = player.transform.position + directionAwayFromPlayer * stayAwayDistance;
            navMeshAgent.SetDestination(movePosition);

        }

        if (distanceToPlayer <= tooCloseDistance && timer > maxTimeBetweenAttacks)
        {
            aimHigh = false;
            Attack();
        }

        else if (distanceToPlayer <= stayAwayDistance + 1.5f && timer > maxTimeBetweenAttacks)
        {
            navMeshAgent.SetDestination(transform.position);
            aimHigh = true;
            Attack();
        }
    }



    void Attack()
    {
        AimAtTarget();
        timer = 0;
        GameObject cannonball = Instantiate(cannonballPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody cannonballRb = cannonball.GetComponent<Rigidbody>();
        Vector3 direction = spawnPoint.forward;
        cannonballRb.velocity = direction * explosionImpulse;
    }

    void AimAtTarget()
    {
        Vector3 planeTarget = player.transform.position;
        planeTarget.y = spawnPoint.position.y;

        Vector3 direction = (planeTarget - spawnPoint.position).normalized;

        targetRotationY = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);

        transform.rotation = Quaternion.Euler(0, targetRotationY, 0);

        GetAngleToTarget(planeTarget, player.transform.position);

    }

    public void GetAngleToTarget(Vector3 planeTarget, Vector3 targetPos)
    {
        float v = explosionImpulse;
        float g = Mathf.Abs(Physics.gravity.y);

        float x = Vector3.Distance(spawnPoint.position, planeTarget);

        float y = targetPos.y - spawnPoint.position.y;

        float v2 = v * v;
        float v4 = v2 * v2;
        float x2 = x * x;

        float sq = (v4 - g * ((g * x2) + (2.0f * y * v2)));

        if (sq <= 0) 
            return;

        if (aimHigh)
            sq = Mathf.Sqrt(sq);
        else
            sq = -Mathf.Sqrt(sq);

        float angle = Mathf.Atan2((v2 + sq), (g * x));
        targetRotationCannonX = Mathf.Rad2Deg * angle;


        Quaternion currentRotation = spawnPoint.transform.rotation; 
        spawnPoint.transform.rotation = Quaternion.Euler(-targetRotationCannonX, currentRotation.eulerAngles.y, currentRotation.eulerAngles.z);


    }


    void DrawTrajectory()
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
    }

    Vector3 CalculatePositionAtTime(Vector3 startPosition, Vector3 velocity, float time)
    {
        Vector3 position = startPosition + velocity * time + 0.5f * gravity * time * time;
        return position;
    }
}











