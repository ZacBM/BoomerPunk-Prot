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
    public GameObject _armStapler;
    public GameObject _armEmpty;

    public isShooting_animScript _shootingAnim;

    void Start()
    {
        playerBase = GetComponent<PlayerBase>();
        
        _armEmpty.SetActive(true);
        _armStapler.SetActive(false);
        _shootingAnim = _armStapler.GetComponent<isShooting_animScript>();

        meleeRuler = GameObject.FindGameObjectWithTag("Melee Weapon");
        meleeRuler.SetActive(false);
    }

    void Update()
    {
        if (weaponHolder == null) weaponHolder = GameObject.FindGameObjectWithTag("Weapon Holder");
        //if (gunHolder == null) gunHolder = GameObject.Find("Gun Holder");
        bool holdingAWeapon = currentWeapon != null;
        
        if (playerBase.pickUp.triggered)
        {
            if (holdingAWeapon) ThrowCurrentWeapon(); //DropCurrentWeapon();
            else PickUpANearbyWeapon();
        }

        if (playerBase.shoot.triggered)
        {
            if (holdingAWeapon)
            {
                currentWeapon.Shoot();
                if (_armStapler.activeSelf)
                {
                    _shootingAnim.TriggerShootAnimation();
                }
            }
            else
            {
                StartCoroutine(UseMeleeWeapon());
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
                    rangedWeapon.PickUp(weaponHolder);//rangedWeapon.PickUp(gunHolder);
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
        _armEmpty?.SetActive(true);
        _armStapler?.SetActive(false);
        //Jake
    }
    
    void ThrowCurrentWeapon()
    {
        if (cameraTransform == null) cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        if (cameraTransform != null) currentWeapon.Throw(cameraTransform.forward * throwStrength);
        currentWeapon = null;
        //Jake
        _armEmpty?.SetActive(true);
        _armStapler?.SetActive(false);
        //Jake
    }

    IEnumerator UseMeleeWeapon()
    {
        meleeRuler.SetActive(true);
        //if (cameraTransform == null) cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
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