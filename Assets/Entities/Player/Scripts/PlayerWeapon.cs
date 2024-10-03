using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    private GameObject potentialWeapon;
    public LayerMask weaponLayer;
    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == weaponLayer)
        {
            potentialWeapon = collider.gameObject;
        }
    }
    
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == potentialWeapon)
        {
            potentialWeapon = null;
        }
    }
}