using UnityEngine;

public interface RangedWeapon
{
    public void Drop();
    
    public void PickUp(GameObject parent);
    
    public void Throw(Vector3 throwDirection);
    
    public void Shoot();
}