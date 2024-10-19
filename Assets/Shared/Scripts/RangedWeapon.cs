using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(AmmoComponent))]
[RequireComponent(typeof(HitComponent))]
[RequireComponent(typeof(PhysicsComponent))]
[RequireComponent(typeof(SpawnComponent))]

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]

public abstract class RangedWeapon : MonoBehaviour
{
    protected AmmoComponent ammoHolder;
    protected HitComponent hitbox;
    protected PhysicsComponent rigidbodyHelper;
    
    [SerializeField] public Transform bulletOrigin;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float shotRange = 10.0f;
    [SerializeField] protected float bulletForce = 50.0f;
    
    [SerializeField] protected VisualEffect shotVisualEffect;

    protected AudioSource audioSource;
    public AudioClip shootAudio;
    public AudioClip pickupAudio;

    protected Recoil recoil;

    protected bool isShooting = false;
    
    [HideInInspector] public float shotDelay;
    public float shotDelaySet = 0.2f;
    
    protected virtual void Awake()
    {
        ammoHolder = GetComponent<AmmoComponent>();
        hitbox = GetComponent<HitComponent>();
        rigidbodyHelper = GetComponent<PhysicsComponent>();
        
        audioSource = GetComponent<AudioSource>();
        recoil = FindObjectOfType<Recoil>();
    }
    
    protected virtual void Start()
    {
        hitbox.isActive = false;
        
        rigidbodyHelper.ActivateRigidbody();
    }

    protected virtual void Update()
    {
        
    }
    
    public void Drop()
    {
        transform.SetParent(null);
        rigidbodyHelper.ActivateRigidbody();
    }
    
    public void PickUp(GameObject parent)
    {
        rigidbodyHelper.DectivateRigidbody();
        transform.SetParent(parent.transform, false);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (pickupAudio != null)
        {
            audioSource?.PlayOneShot(pickupAudio);
        }

        //Jake
        //parent.SetActive(false);
        //Jake
    }
    
    public void Throw(Vector3 throwDirection)
    {
        transform.SetParent(null);
        rigidbodyHelper.ActivateRigidbody();
        GetComponent<Rigidbody>().AddForce(throwDirection, ForceMode.Impulse);
    }
    
    void CreateVisualEffect()
    {
        shotVisualEffect?.Play();
    }
    
    public void InstantiateBullet()
    {
        if (bulletPrefab != null)
        {
            GameObject spawnedBullet = Instantiate(bulletPrefab, bulletOrigin.position, bulletOrigin.rotation);

            // Apply force to the bullet in the correct direction
            Rigidbody bulletRigidbody = spawnedBullet?.GetComponent<Rigidbody>();
            bulletRigidbody?.AddForce(bulletOrigin.forward * bulletForce, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(bulletOrigin.position, shotRange);
        Gizmos.DrawLine(bulletOrigin.position, bulletOrigin.position + (bulletOrigin.forward * shotRange));
    }

    public virtual void Shoot()
    {
        if (ammoHolder.IsEmpty())
        {
            Debug.Log("This weapon's out of ammo!");
            return;
        }
        ammoHolder.UseAmmo();
        
        InstantiateBullet();
        if (shotVisualEffect != null)
        {
            CreateVisualEffect();
        }

        recoil.recoil();
        if (shootAudio != null)
        {
            audioSource?.PlayOneShot(shootAudio);
        }
    }

    public void PrepareToShoot()
    {
        isShooting = true;
    }
    
    public void PrepareToStop()
    {
        isShooting = false;
    }
}