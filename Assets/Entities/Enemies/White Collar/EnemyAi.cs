using System;
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
    [SerializeField] float stayAwayDistance;
    public static List<EnemyAi> enemiesInAttackRange = new();
    public static int maxAttackers = 4;

    float shuffleSpeed;
    float shuffleAmplitude;
    [SerializeField] float force;
    Rigidbody rb;
    [SerializeField] float timeToDie;

    [Header("Death Sounds")]
    public AudioClip[] deathSounds;

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
        //rb.isKinematic = false;
        player = GameObject.FindWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        shuffleSpeed = Random.Range(1.0f, 3.0f);
        shuffleAmplitude = Random.Range(1.0f, 3.0f);
    }

    // Update is called once per frame
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
                //player.GetComponent<HPComponent>().ChangeHealth(-1);
                //deal damage on timer
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

    public void PlayDeathSound()
    {
        if (deathSounds != null)
        {
            if (deathSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, deathSounds.Length);
                AudioSource.PlayClipAtPoint(deathSounds[randomIndex], transform.position);
                //Debug.Log("Death Sound");
            }
        }
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

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Melee Weapon")
        {
            SmallKnockback();
            PlayDeathSound();
            Invoke("TrackAgain", (timeToDie / 2f));
        }
        if (otherCollider.gameObject.tag == "Thrown Weapon")
        {
            SmallKnockback();
            PlayDeathSound();
            Invoke("TrackAgain", (timeToDie / 2f));
        }
    }
}