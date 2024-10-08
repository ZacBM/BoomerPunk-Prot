using System;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class Weapon : MonoBehaviour
{
    [SerializeField] public Transform bulletOrigin;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int ammo = 10;
    [SerializeField] private float shotRange = 10.0f;
    [SerializeField] private float bulletForce = 50.0f;
    
    [SerializeField] private VisualEffect shotVisualEffect;

    private Recoil recoil;
    void Start()
    {
        ActivateRigidbody();
        //recoil = GetComponent<Recoil>();
        recoil = FindObjectOfType<Recoil>();

    }

    public void Shoot()
    {
        ammo--;
        RaycastHit hit;
        Physics.Raycast(bulletOrigin.position, bulletOrigin.forward, out hit, shotRange);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.TryGetComponent<HPComponent>(out HPComponent hpc)) hpc.ChangeHealth(-1);
        }
        //Debug.DrawRay(bulletOrigin.position, bulletOrigin.forward * shotRange, Color.red, 1.0f);
        InstantiateBullet();
        if (shotVisualEffect != null) CreateVisualEffect();

        recoil.recoil();
    }

    void CreateVisualEffect()
    {
        shotVisualEffect.Play();
    }

    public float GetBulletForce()
    {
        return bulletForce;
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
        //Jake
        //parent.SetActive(false);
        //Jake
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

    public void InstantiateBullet()
    {
        if (bullet != null)
        {
            // Instantiate the bullet at correct, origin position and rotation
            GameObject spawnedBullet = Instantiate(bullet, bulletOrigin.position, bulletOrigin.rotation);

            // Apply force to the bullet in the correct direction
            Rigidbody bulletRb = spawnedBullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.AddForce(bulletOrigin.forward * GetBulletForce(), ForceMode.Impulse);
                //Debug.Log("Bullet instantiated and force applied.");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(bulletOrigin.position, shotRange);
    }
}