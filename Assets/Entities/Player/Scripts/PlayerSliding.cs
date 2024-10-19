using UnityEngine;

[RequireComponent(typeof(PlayerBase))]

public class PlayerSliding : MonoBehaviour
{
    private PlayerBase playerBase;

    [Header("References")] public Transform cameraTransform;
    public Transform playerTransform;
    private Rigidbody rb;

    [Header("Sliding")] public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")] private float horizontalInput;
    private float verticalInput;

    [Header("Speed Change")] public float initialSpeedMultiplier = 15f;
    public float speedDecay = 0.1f;
    private float currentSpeedMultiplier;

    public bool isSliding;

    private void Awake()
    {
        playerBase = GetComponent<PlayerBase>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        startYScale = playerTransform.localScale.y;
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
        if (isSliding)
        {
            return;
        }

        isSliding = true;
        playerTransform.localScale =
            new Vector3(playerTransform.localScale.x, slideYScale, playerTransform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
        currentSpeedMultiplier = initialSpeedMultiplier;
    }

    void SlidingMovement()
    {
        Vector3 inputDirection = cameraTransform.forward * verticalInput + cameraTransform.right * horizontalInput;
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
        isSliding = false;
        playerTransform.localScale =
            new Vector3(playerTransform.localScale.x, startYScale, playerTransform.localScale.z);
        currentSpeedMultiplier = 1.0f;
    }
}
