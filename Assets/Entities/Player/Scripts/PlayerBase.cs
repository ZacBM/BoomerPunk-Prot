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
    [HideInInspector] public TextMeshProUGUI healthText;

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
        
        if (healthText == null)
        {
            healthText = GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>();
        }

        if (hpComponent != null && healthText != null)
        {
            healthText.text = "Health: " + hpComponent.health;
        }
        else if (healthText != null)
        {
            healthText.text = "Health: 0";
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
            GameManager.gameManager.ReloadScene();
        }
    }
}
