using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoWeaponSwitcher : MonoBehaviour
{
    public GameObject weaponhand;

    // Start is called before the first frame update
    void Start()
    {
        if (weaponhand == null)
        {
            weaponhand.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchWeapon()
    {
        
    }
}
