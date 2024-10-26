using UnityEngine;

[RequireComponent(typeof(Collider))]

public class HitComponent : MonoBehaviour
{
    /// <summary>  
    /// A component that acts as a hitbox.
    ///
    /// The HitComponent handles interaction with objects that possess HPComponents. If an interaction takes place, the
    /// current health of the HPComponent is decremented by a specified amount.
    /// /// - Joshua  
    /// </summary>  
    
    public Collider hitboxCollider;
    
    public bool isActive = true;
    public int hitStrength = 1;

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (isActive && otherCollider.TryGetComponent<HPComponent>(out HPComponent hpComponent))
        {
            hpComponent.ChangeHealth(-hitStrength);
        }
    }
}
