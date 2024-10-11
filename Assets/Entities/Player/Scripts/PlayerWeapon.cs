using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private RangedWeapon currentWeapon;
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private GameObject gunHolder;
    [SerializeField] private Transform cameraTransform;
    
    [SerializeField] private float maximumPickUpDistance = 3.0f;
    [SerializeField] private float throwStrength = 20.0f;
    
    [SerializeField] private KeyCode pickupKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;

    // Jake: IN CASE I FUCKED SOMETHING UP LMK
    public GameObject _armStapler;
    public GameObject _armEmpty;

    public isShooting_animScript _shootingAnim;

    void Start()
    {
        _armEmpty.SetActive(true);
        _armStapler.SetActive(false);
        _shootingAnim = _armStapler.GetComponent<isShooting_animScript>();
    }

    void Update()
    {
        if (weaponHolder == null) weaponHolder = GameObject.FindGameObjectWithTag("Weapon Holder");
        if (gunHolder == null) gunHolder = GameObject.Find("Gun Holder");
        bool holdingAWeapon = currentWeapon != null;
        
        if (Input.GetKeyDown(pickupKey))
        {
            if (holdingAWeapon) ThrowCurrentWeapon(); //DropCurrentWeapon();
            else PickUpANearbyWeapon();
        }

        if (Input.GetKeyDown(shootKey) && holdingAWeapon)
        {
            currentWeapon.Shoot();
            if (_armStapler.activeSelf)
            {
                _shootingAnim.TriggerShootAnimation();
            }
        }
    }

    void PickUpANearbyWeapon()
    {
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject nearbyObject in allGameObjects)
        {
            if (nearbyObject.TryGetComponent<RangedWeapon>(out RangedWeapon rangedWeapon))
            {
                if (Vector3.Distance(nearbyObject.transform.position, transform.position) < maximumPickUpDistance)
                {
                    rangedWeapon.PickUp(gunHolder);
                    currentWeapon = rangedWeapon;
                    //Jake
                    _armEmpty.SetActive(false);
                    _armStapler.SetActive(true);
                    //Jake
                    break;
                }
            }
        }
    }

    void DropCurrentWeapon()
    {
        currentWeapon.Drop();
        currentWeapon = null;
        //Jake
        _armEmpty.SetActive(true);
        _armStapler.SetActive(false);
        //Jake
    }
    
    void ThrowCurrentWeapon()
    {
        if (cameraTransform == null) cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        if (cameraTransform != null) currentWeapon.Throw(cameraTransform.forward * throwStrength);
        currentWeapon = null;
        //Jake
        _armEmpty.SetActive(true);
        _armStapler?.SetActive(false);
        //Jake
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, maximumPickUpDistance);
    }
}