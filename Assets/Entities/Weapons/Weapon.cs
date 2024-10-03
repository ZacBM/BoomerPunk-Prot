using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform bulletOrigin;
    
    [SerializeField] private int ammo = 10;
    [SerializeField] private float shotRange = 10.0f;
    
    void Start()
    {
        ActivateRigidbody();
    }

    public void Shoot()
    {
        RaycastHit hit;
        Physics.Raycast(bulletOrigin.position, bulletOrigin.forward, out hit, shotRange);
        if (hit.collider != null) Destroy(hit.collider.gameObject); // Right now, this only deletes what it hits.
    }
    
    void ActivateRigidbody()
    {
        GetComponent<Rigidbody>().detectCollisions = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    
    void DectivateRigidbody()
    {
        GetComponent<Rigidbody>().detectCollisions = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
    
    public void GetPickedUp(GameObject parent)
    {
        DectivateRigidbody();
        transform.SetParent(parent.transform, false);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void GetDropped()
    {
        transform.SetParent(null);
        ActivateRigidbody();
    }
    
    public void GetThrown(Vector3 throwDirection)
    {
        transform.SetParent(null);
        ActivateRigidbody();
        GetComponent<Rigidbody>().AddForce(throwDirection, ForceMode.Impulse);
    }
}