using UnityEngine;

public interface RangedWeapon
{
    /// <summary>
    /// An interface that denotes ranged weapons, & defines common methods for weapons.
    ///
    /// - Joshua
    /// </summary>
    
    public void Drop();
    
    public void PickUp(GameObject parent);
    
    public void Throw(Vector3 throwDirection);
    
    public void Shoot();

    public void PrepareToShoot();
    
    public void PrepareToStop();
}