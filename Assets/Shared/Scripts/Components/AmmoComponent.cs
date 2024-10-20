using UnityEngine;

public class AmmoComponent : MonoBehaviour
{
    [SerializeField] private int ammoLeft = 10;
    
    public int GetAmmoLeft() => ammoLeft;
    
    public bool IsEmpty() => ammoLeft == 0;

    public int UseAmmo(int amountOfAmmoToUse = 1)
    {
        if (!IsEmpty()) ammoLeft -= amountOfAmmoToUse;
        return ammoLeft;
    }
}
