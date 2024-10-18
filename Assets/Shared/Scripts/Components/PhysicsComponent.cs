using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]

public class PhysicsComponent : MonoBehaviour
{
    public Rigidbody rigidbody;

    public float secondsToRecoverFromKnockback;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    
    public void ActivateRigidbody()
    {
        rigidbody.detectCollisions = true;
        rigidbody.isKinematic = false;
    }
    
    public void DectivateRigidbody()
    {
        rigidbody.detectCollisions = false;
        rigidbody.isKinematic = true;
    }
    
    public void Knockback(float horizontalKnockbackStrength, float verticalKnockbackStrength, Vector3 knockbackerPosition)
    {
        rigidbody.isKinematic = false;

        Vector3 forceDirection = (transform.position - knockbackerPosition).normalized;

        rigidbody.AddForce(forceDirection * horizontalKnockbackStrength, ForceMode.Impulse);
        rigidbody.AddForce(Vector3.up * verticalKnockbackStrength, ForceMode.Impulse);
        
        if (TryGetComponent<NavMeshAgent>(out NavMeshAgent navMeshAgent))
        {
            navMeshAgent.enabled = false;
        }
        
        Invoke(nameof(RecoverFromKnockback), secondsToRecoverFromKnockback);
    }

    public void RecoverFromKnockback()
    {
        rigidbody.isKinematic = true;
        
        if (TryGetComponent<NavMeshAgent>(out NavMeshAgent navMeshAgent))
        {
            navMeshAgent.enabled = true;
        }
    }
}
