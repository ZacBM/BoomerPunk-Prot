using UnityEngine;

[RequireComponent(typeof(Collider))]

public class HitComponent : MonoBehaviour
{
    public Collider hitboxCollider;
    
    public bool isActive = true;

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (isActive && otherCollider.TryGetComponent<HPComponent>(out HPComponent hpComponent))
        {
            hpComponent.ChangeHealth(-1);
        }
    }
}
