using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerBase))]

public class PlayerWeapon : MonoBehaviour
{
    private PlayerBase playerBase;
    
    [SerializeField] private RangedWeapon currentWeapon;
    [SerializeField] private GameObject weaponHolder;
    //[SerializeField] private GameObject gunHolder;
    [SerializeField] private Transform cameraTransform;
    private GameObject meleeRuler;
    
    [SerializeField] private float maximumPickUpDistance = 3.0f;
    [SerializeField] private float throwStrength = 20.0f;

    // Jake: IN CASE I FUCKED SOMETHING UP LMK
    //jake
    public GameObject hands;

    public isShooting_animScript _shootingAnim;

    void Start()
    {
        playerBase = GetComponent<PlayerBase>();

        meleeRuler = GameObject.FindGameObjectWithTag("Melee Weapon");
        meleeRuler.SetActive(false);
        
        hands = GameObject.Find("Hands");
        _shootingAnim = hands.GetComponent<isShooting_animScript>();
    }

    void Update()
    {
        if (weaponHolder == null)
        {
            weaponHolder = GameObject.FindGameObjectWithTag("Weapon Holder");
        }
        //if (gunHolder == null) gunHolder = GameObject.Find("Gun Holder");
        bool holdingAWeapon = currentWeapon != null;
        
        if (playerBase.pickUp.triggered)
        {
            if (holdingAWeapon)
            {
                ThrowCurrentWeapon(); //DropCurrentWeapon();
            }
            else
            {
                PickUpANearbyWeapon();
            }
        }

        if (playerBase.shoot.triggered)
        {
            if (holdingAWeapon)
            {
                currentWeapon.PrepareToShoot();
                if (hands.activeSelf)
                {
                    
                    _shootingAnim.TriggerShootAnimation();
                }
            }
            else
            {
                StartCoroutine(UseMeleeWeapon());
            }
        }
        
        if (playerBase.shoot.WasReleasedThisFrame())
        {
            if (holdingAWeapon)
            {
                currentWeapon.PrepareToStop();
            }
        }
    }

    void PickUpANearbyWeapon()
    {
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject nearbyObject in allGameObjects)
        {
            if (nearbyObject.TryGetComponent<RangedWeapon>(out RangedWeapon rangedWeapon))
            {
                if (Vector3.Distance(nearbyObject.transform.position, transform.position) < maximumPickUpDistance)
                {
                    rangedWeapon.PickUp(weaponHolder);//rangedWeapon.PickUp(gunHolder);
                    currentWeapon = rangedWeapon;

                    GameObject weaponGameObject = (currentWeapon as MonoBehaviour).gameObject;

                    if (weaponGameObject.name.Contains("Stapler"))
                    {
                        _shootingAnim.EquipStapler();
                    }
                    else if (weaponGameObject.name.Contains("Tack"))
                    {
                        _shootingAnim.EquipTac();
                    }

                    if (nearbyObject.TryGetComponent<AmmoComponent>(out AmmoComponent ammoComponent))
                    {
                        playerBase.weapomAmmoComponent = ammoComponent;
                    }

                    break;
                }
            }
        }
    }

    void DropCurrentWeapon()
    {
        currentWeapon?.Drop();
        currentWeapon = null;
        
        playerBase.weapomAmmoComponent = null;
    }

    void ThrowCurrentWeapon()
    {
        if (cameraTransform == null)
        {
            cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        currentWeapon?.Throw(cameraTransform.forward * throwStrength);

        GameObject weaponGameObject = (currentWeapon as MonoBehaviour).gameObject;

        if (weaponGameObject.name.Contains("Stapler"))
        {
            _shootingAnim.UnequipStapler();
        }
        else if (weaponGameObject.name.Contains("Tack"))
        {
            _shootingAnim.UnequipTac();
        }

        currentWeapon = null;
        playerBase.weapomAmmoComponent = null;
    }

    IEnumerator UseMeleeWeapon()
    {
        meleeRuler.SetActive(true);
        meleeRuler.transform.position = weaponHolder.transform.position;
        meleeRuler.transform.localEulerAngles = weaponHolder.transform.eulerAngles + new Vector3(0f, -90f, 0f);
        for (int i = 0; i < 27; i++)
        {
            meleeRuler.transform.localEulerAngles += new Vector3(0f, 5f, 0f);
            yield return new WaitForSeconds(1f / 60f);
        }

        meleeRuler.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, maximumPickUpDistance);
    }
}