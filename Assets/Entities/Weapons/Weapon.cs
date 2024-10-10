// Base weapon class removed; to be fully replace with components & interfaces.

/*using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] public Transform bulletOrigin;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected int ammo = 10;
    [SerializeField] protected float shotRange = 10.0f;
    [SerializeField] protected float bulletForce = 50.0f;
    
    [SerializeField] protected VisualEffect shotVisualEffect;

    public AudioSource audioSource;
    public AudioClip shootAudio;
    public AudioClip pickupAudio;

    protected Recoil recoil;
    
    void Start()
    {
        ActivateRigidbody();
        //recoil = GetComponent<Recoil>();
        recoil = FindObjectOfType<Recoil>();

        audioSource = GetComponent<AudioSource>(); 
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Ensure AudioSource exists
        }
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
        if (shootAudio != null) audioSource.PlayOneShot(shootAudio);
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

        if (pickupAudio != null) audioSource.PlayOneShot(pickupAudio);

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

    public abstract void InstantiateBullet();

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(bulletOrigin.position, shotRange);
    }
}*/