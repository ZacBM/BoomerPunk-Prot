using UnityEngine;

[RequireComponent(typeof(PlayerBase))]

public class PlayerHeadBop : MonoBehaviour
{
    //head bop variables
    public float bobbingSpeed = 12.5f;
    public float bobbingAmount = 0.115f;
    public float idleBobbingSpeed = 4.0f;
    public float idleBobbingAmount = 0.02f;

    public GameObject camera;

    private float defaultBobbingAmount;
    private float defaultBobbingSpeed;
    private Vector3 originalCameraPosition;

    private PlayerBase playerBase;
    private PlayerMovement playerMovement;
    private PlayerSliding playerSliding;

    private void Awake()
    {
        playerBase = GetComponent<PlayerBase>();
        playerMovement = GetComponent<PlayerMovement>();
        playerSliding = GetComponent<PlayerSliding>();
    }

    private void Start()
    {
        originalCameraPosition = camera.transform.localPosition;

        defaultBobbingAmount = bobbingAmount;
        defaultBobbingSpeed = bobbingSpeed;
    }

    void Update()
    {
        bool isAirborne = !playerMovement.IsGrounded();
        if (playerSliding.isSliding || isAirborne)
        {
            camera.transform.localPosition = originalCameraPosition;
            return;
        }

        float horizontal = playerBase.movementDirection.x;
        float vertical = playerBase.movementDirection.y;

        bool isMoving = Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0;
        float waveslice = Mathf.Sin(Time.time * bobbingSpeed);
        float translateChange = 0.0f;

        if (isMoving)
        {
            //Bobbing Math
            // Increase bobbing when the player moves
            //waveslice = Mathf.Sin(Time.time * bobbingSpeed);
            translateChange = waveslice * bobbingAmount;

            float totalAxes = Mathf.Clamp(Mathf.Abs(horizontal) + Mathf.Abs(vertical), 0.0f, 1.0f);
            translateChange *= totalAxes;
        }
        else
        {
            // Subtle bobbing when not moving
            waveslice = Mathf.Sin(Time.time * idleBobbingSpeed);
            translateChange = waveslice * idleBobbingAmount;
        }

        // Actually moves the camera
        camera.transform.localPosition = new Vector3(originalCameraPosition.x, originalCameraPosition.y + translateChange, originalCameraPosition.z);
    }
}
