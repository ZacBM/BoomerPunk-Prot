using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]

public class PhysicsComponent : MonoBehaviour
{
    private Rigidbody rigidbody;

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
        if (TryGetComponent<NavMeshAgent>(out NavMeshAgent navMeshAgent))
        {
            navMeshAgent.enabled = false;
        }
        rigidbody.isKinematic = false;

        Vector3 forceDirection = (transform.position - knockbackerPosition).normalized;

        rigidbody.AddForce(forceDirection * horizontalKnockbackStrength, ForceMode.Impulse);
        rigidbody.AddForce(Vector3.up * verticalKnockbackStrength, ForceMode.Impulse);
    }
}
