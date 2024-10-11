using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class RigidbodyHelperComponent : MonoBehaviour
{
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
