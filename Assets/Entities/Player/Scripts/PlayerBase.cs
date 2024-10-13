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
    public InputAction pickUp;
    public InputAction shoot;
    
    [HideInInspector] public Vector3 movementDirection = Vector3.zero;

    [HideInInspector] public bool dashPerformed;
    [HideInInspector] public bool jumpPerformed;
    [HideInInspector] public bool pickUpPerformed;
    [HideInInspector] public bool shootPerformed;

    void OnEnable()
    {
        move.Enable();

        dash.Enable();
        dash.performed += ctx => dashPerformed = true;
        jump.Enable();
        jump.performed += ctx => jumpPerformed = true;
        pickUp.Enable();
        pickUp.performed += ctx => pickUpPerformed = true;
        shoot.Enable();
        shoot.performed += ctx => shootPerformed = true;
    }

    void OnDisable()
    {
        move.Disable();
        dash.Disable();
        jump.Disable();
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
    }
    
    void ReceiveInput()
    {
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
