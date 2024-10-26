using UnityEngine;

public class AmmoComponent : MonoBehaviour
{
    /// <summary>  
    /// A component that manages an ammo variable.
    ///
    /// The AmmoComponent will usually be seen on ranged weapons.
    /// /// - Joshua  
    /// </summary>  
  
    /// <remarks>  
    /// Changes to make:
    /// - Make "UseAmmo()" the only means of mutating "ammoLeft", to boost reasonability
    /// - - If reload functionality is desired, a "Reload()" function can also mutate "ammoLeft"
    ///  
    /// - Joshua  
    /// </remarks>
    
    public int ammoLeft = 10;
    
    public bool IsEmpty()
    {
        return ammoLeft <= 0;
    }

    public int UseAmmo(int amountOfAmmoToUse = 1)
    {
        if (!IsEmpty()) ammoLeft -= amountOfAmmoToUse;
        return ammoLeft;
    }
}
