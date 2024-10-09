using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private GameObject gunHolder;
    [SerializeField] private Transform cameraTransform;
    
    [SerializeField] private float maximumPickUpDistance = 3.0f;
    [SerializeField] private float throwStrength = 20.0f;
    
    [SerializeField] private KeyCode pickupKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;

    //Jake IN CASE I FUCKED SOMETHING UP LMK
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
        Weapon[] nearbyObjects = FindObjectsOfType<Weapon>();
        foreach (Weapon weapon in nearbyObjects)
        {
            if (Vector3.Distance(weapon.gameObject.transform.position, transform.position) < maximumPickUpDistance)
            {
                weapon.GetPickedUp(gunHolder);
                currentWeapon = weapon;
                //Jake
                _armEmpty.SetActive(false);
                _armStapler.SetActive(true);
                //Jake
                break;
            }
        }
    }

    void DropCurrentWeapon()
    {
        currentWeapon.GetDropped();
        currentWeapon = null;
        //Jake
        _armEmpty.SetActive(true);
        _armStapler.SetActive(false);
        //Jake
    }
    
    void ThrowCurrentWeapon()
    {
        if (cameraTransform == null) cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        if (cameraTransform != null) currentWeapon.GetThrown(cameraTransform.forward * throwStrength);
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