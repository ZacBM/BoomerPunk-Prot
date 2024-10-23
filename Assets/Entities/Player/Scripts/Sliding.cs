using UnityEngine;

[RequireComponent(typeof(PlayerBase))]

public class Sliding : MonoBehaviour
{
    /// <remarks>
    /// Changes to make:
    /// - Remove script & move logic to the PlayerMovement script
    /// 
    /// - Joshua  
    /// </remarks>
    
    private PlayerBase playerBase;
    
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header("Speed Change")]
    public float initialSpeedMultiplier = 15f;
    public float speedDecay = 0.1f;
    private float currentSpeedMultiplier;

    public bool sliding;

    void Start()
    {
        playerBase = GetComponent<PlayerBase>();
        
        rb = GetComponent<Rigidbody>();

        startYScale = playerObj.localScale.y;
    }

    void Update()
    {
        horizontalInput = playerBase.movementDirection.x;
        verticalInput = playerBase.movementDirection.y;

        if (playerBase.dash.triggered && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide();
        }

        if (playerBase.dash.WasReleasedThisFrame())
        {
            StopSlide();
        }
    }

    void FixedUpdate()
    {
        SlidingMovement();
    }

    void StartSlide()
    {
        if (sliding)
        {
            return;
        }

        sliding = true;
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
        currentSpeedMultiplier = initialSpeedMultiplier;
    }

    void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
        slideTimer -= Time.deltaTime;
        currentSpeedMultiplier -= speedDecay * Time.deltaTime;

        if (slideTimer <= 0)
        {
            StopSlide();
        }
    }

    public float GetCurrentSpeedMultiplier()
    {
        currentSpeedMultiplier = Mathf.Max(1.0f, currentSpeedMultiplier - (speedDecay * Time.deltaTime));

        return currentSpeedMultiplier;
    }

    void StopSlide()
    {
        sliding = false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
        currentSpeedMultiplier = 1.0f;
    }
}
