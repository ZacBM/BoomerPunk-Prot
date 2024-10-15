using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HPComponent))]

public class PlayerBase : MonoBehaviour
{
    /// <summary>
    /// The "main" player script. The idea place for logic & behavior that other player scripts shouldn't care
    /// about.
    /// </summary>
    
    [Header("Components")]
    [SerializeField] private HPComponent hpComponent;
    
    [Header("Input")]
    public InputAction move;
    public InputAction dash;
    public InputAction jump;
    public InputAction look;
    public InputAction pickUp;
    public InputAction shoot;
    
    [HideInInspector] public Vector3 movementDirection = Vector3.zero;
    [HideInInspector] public Vector3 lookDirection = Vector3.zero;

    /*[HideInInspector] public bool dashPerformed;
    [HideInInspector] public bool jumpPerformed;
    [HideInInspector] public bool pickUpPerformed;
    [HideInInspector] public bool shootPerformed;*/
    
    [Header("Weapon Display")]
    [HideInInspector] public AmmoComponent weapomAmmoComponent;
    [HideInInspector] public TextMeshProUGUI ammoText;

    void OnEnable()
    {
        move.Enable();
        dash.Enable();
        jump.Enable();
        look.Enable();
        pickUp.Enable();
        shoot.Enable();
        
        /*dash.performed += ctx => dashPerformed = true;
        jump.performed += ctx => jumpPerformed = true;
        pickUp.performed += ctx => pickUpPerformed = true;
        shoot.performed += ctx => shootPerformed = true;*/
    }

    void OnDisable()
    {
        move.Disable();
        dash.Disable();
        jump.Disable();
        look.Disable();
        pickUp.Disable();
        shoot.Disable();
    }

    void Start()
    {
        hpComponent = GetComponent<HPComponent>();
    }
    
    void Update()
    {
        ResetSceneIfDead();
        ReceiveInput();

        if (ammoText == null)
        {
            ammoText = GameObject.Find("Ammo Text").GetComponent<TextMeshProUGUI>();
        }

        if (weapomAmmoComponent != null && ammoText != null)
        {
            ammoText.text = "Ammo: " + weapomAmmoComponent.ammoLeft;
        }
        else if (ammoText != null)
        {
            ammoText.text = "Ammo: 0";
        }
    }
    
    void ReceiveInput()
    {
        lookDirection = look.ReadValue<Vector2>();
        movementDirection = move.ReadValue<Vector2>();
    }

    void ResetSceneIfDead()
    {
        if (hpComponent.IsDead())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

/*
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
   
   using UnityEngine;
   using UnityEngine.Events;
   
   public class HPComponent : MonoBehaviour
   {
       [Range(0, 200)]
       public int health = 100;
       [Range(0, 200)]
       public int maxHealth = 100;
       
       [SerializeField] enum BehaviorsOnDeath {DEACTIVATE, DELETE, NOTHING}
       [SerializeField] BehaviorsOnDeath behaviorOnDeath = BehaviorsOnDeath.DELETE;
   
       private EnemyAi enemyAi;
   
       public UnityEvent healthChange;
   
       private void Awake()
       {
           enemyAi = GetComponent<EnemyAi>();
           if (enemyAi != null)
           {
               healthChange.AddListener(enemyAi.OnDeath);
           }
       }
   
       public int ChangeHealth(int changeInHealth)
       {
           health += changeInHealth;
           if (IsDead())
           {
               Die();
           }
   
           if (health > maxHealth)
           {
               health = maxHealth;
           }
           return health;
       }
   
       public bool IsDead()
       {
           return health <= 0;
       }
   
       void Die()
       {
           if (enemyAi != null)
           {
               enemyAi.PlayDeathSound();
               healthChange.Invoke();
           }
   
           if (behaviorOnDeath == BehaviorsOnDeath.DEACTIVATE)
           {
               gameObject.SetActive(false);
           }
           else if (behaviorOnDeath == BehaviorsOnDeath.DELETE)
           {
               Destroy(gameObject);
           }
       }
   }
   
   /*
    * Derived from a tutorial by Dave / GameDevelopment
    * https://www.youtube.com/watch?v=f473C43s8nE&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=7
    * /
   
   using UnityEngine;
   
   [RequireComponent(typeof(PlayerBase))]
   
   public class PlayerMovement : MonoBehaviour
   {
   	private PlayerBase playerBase;
   	
   	[Header("Movement")]
   	public float moveSpeed;
   
   	public float groundDrag;
   
   	public float jumpForce;
   	public float airMultiplier;
   
   	[Header("Ground Check")]
   	public float playerHeight;
   	public LayerMask groundLayer;
   	public bool grounded;
   
   	public Transform orientation;
   
   	float horizontalInput;
   	float verticalInput;
   
   	Vector3 moveDirection;
   
   	Rigidbody rigidbody;
   	
   	public LayerMask exitLayer;
   
   	private Sliding sliding;
   
   	[SerializeField] private float gravityScale;
   
       private void Start()
   	{
   		playerBase = GetComponent<PlayerBase>();
   		
   		rigidbody = GetComponent<Rigidbody>();
   		rigidbody.freezeRotation = true;
   
           sliding = GetComponent<Sliding>();
       }
   	
   	private void Update()
   	{
   		// Check to see if the player is touching the ground.
   		grounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
   
   		MyInput();
   		SpeedControl();
   
   		//Handling the drag.
   		if (grounded)
   		{
   			rigidbody.drag = groundDrag;
   		}
   		else
   		{
   			rigidbody.drag = 0f;
   		}
       }
   
   	private void FixedUpdate()
   	{
   		MovePlayer();
   	}
   
   	private void MyInput()
   	{
   		horizontalInput = playerBase.movementDirection.x;
   		verticalInput = playerBase.movementDirection.y;
   
   		// When to jump.
   		if (playerBase.jump.triggered && grounded)
   		{
   			Jump();
   		}
   	}
   
   	private void ApplyGravity()
   	{
   		Vector3 gravity = Physics.gravity * gravityScale;
   		rigidbody.AddForce(gravity, ForceMode.Acceleration);
   	}
   
   	private void MovePlayer()
   	{
   		// Calculate movement direction.
   		moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
   
           float appliedMoveSpeed = moveSpeed;
   
           if (sliding != null && sliding.sliding)
           {
   	        appliedMoveSpeed *= sliding.GetCurrentSpeedMultiplier();
           }
   
           //On Ground
           if (grounded)
           {
   	        rigidbody.AddForce(moveDirection.normalized * appliedMoveSpeed * 10f, ForceMode.Force);
           }
   
   		//in air
   		else if (!grounded)
   		{
   			rigidbody.AddForce(moveDirection.normalized * appliedMoveSpeed * 10f * airMultiplier, ForceMode.Force);
   		}
           
           ApplyGravity();
       }
   
   	private void SpeedControl()
   	{
   		Vector3 flatVelocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
   
   		// Limits the velocity if needed.
   		if (flatVelocity.magnitude > moveSpeed)
   		{
   			Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
   			rigidbody.velocity = new Vector3(limitedVelocity.x, rigidbody.velocity.y, limitedVelocity.z);
   		}
   	}
   
   	private void Jump()
   	{
   		// Reset vertical velocity.
   		rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
   		
   		// Send player upwards.
   		rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
   	}
   }
   
 */
