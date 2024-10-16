using UnityEngine;

public class AmmoComponent : MonoBehaviour
{
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
