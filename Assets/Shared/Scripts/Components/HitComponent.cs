using UnityEngine;

[RequireComponent(typeof(Collider))]

public class HitComponent : MonoBehaviour
{
    /// <summary>
    /// Hitbox functionality.
    ///
    /// When an object with a hitbox component comes into contact with an object with an HPComponent, the
    /// HPComponent's health value will decrease.
    /// </summary>
    
    //public Collider hitboxCollider;
    
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
