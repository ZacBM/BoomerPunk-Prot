using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    [Header("Speed Change")]
    public float initialSpeedMultiplier = 15f;
    public float speedDecay = 0.1f;
    private float currentSpeedMultiplier;

    public bool sliding;

    private PlayerMovement PlayerMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        startYScale = playerObj.localScale.y;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
            StartSlide();

        if (Input.GetKeyUp(slideKey))
            StopSlide();

    }

    void FixedUpdate()
    {
        SlidingMovement();
    }

    void StartSlide()
    {
        if (sliding)
            return;

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
            StopSlide();
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
