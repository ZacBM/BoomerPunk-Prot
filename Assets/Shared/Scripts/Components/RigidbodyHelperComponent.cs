using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class RigidbodyHelperComponent : MonoBehaviour
{
    /// <summary>
    /// Helper methods for objects with Rigidbodies.
    ///
    /// - Joshua
    /// </summary>
  
    /// <remarks>
    /// Changes to make:
    /// - Rename to "PhysicsComponent"
    /// 
    /// - Joshua  
    /// </remarks>
    
    public void ActivateRigidbody()
    {
        GetComponent<Rigidbody>().detectCollisions = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    
    public void DectivateRigidbody()
    {
        GetComponent<Rigidbody>().detectCollisions = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
