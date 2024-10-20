using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(AmmoComponent))]
[RequireComponent(typeof(HitComponent))]
[RequireComponent(typeof(PhysicsComponent))]
[RequireComponent(typeof(SpawningComponent))]

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]

public abstract class RangedWeapon : MonoBehaviour
{
    protected AmmoComponent _ammoHolder;
    protected HitComponent _hitbox;
    protected PhysicsComponent _rigidbodyHelper;
    
    [SerializeField] public Transform bulletOrigin;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float shotRange = 10.0f;
    [SerializeField] protected float bulletForce = 50.0f;
    
    [SerializeField] protected VisualEffect shotVisualEffect;

    protected AudioSource _audioSource;
    public AudioClip shootAudio;
    public AudioClip pickupAudio;

    protected Recoil _recoil;

    protected bool _isShooting = false;
    
    [HideInInspector] public float shotDelay;
    public float shotDelaySet = 0.2f;
    
    protected virtual void Awake()
    {
        _ammoHolder = GetComponent<AmmoComponent>();
        _hitbox = GetComponent<HitComponent>();
        _rigidbodyHelper = GetComponent<PhysicsComponent>();
        
        _audioSource = GetComponent<AudioSource>();
        _recoil = FindObjectOfType<Recoil>();
    }
    
    protected virtual void Start()
    {
        _hitbox.isActive = false;
        
        _rigidbodyHelper.ActivateRigidbody();
    }
    
    public void Drop()
    {
        transform.SetParent(null);
        _rigidbodyHelper.ActivateRigidbody();
    }
    
    public void PickUp(GameObject parent)
    {
        _rigidbodyHelper.DectivateRigidbody();
        transform.SetParent(parent.transform, false);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (pickupAudio != null)
        {
            _audioSource?.PlayOneShot(pickupAudio);
        }

        //Jake
        //parent.SetActive(false);
        //Jake
    }
    
    public void Throw(Vector3 throwDirection)
    {
        transform.SetParent(null);
        _rigidbodyHelper.ActivateRigidbody();
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
        if (_ammoHolder.IsEmpty())
        {
            Debug.Log("This weapon's out of ammo!");
            return;
        }
        _ammoHolder.UseAmmo();
        
        InstantiateBullet();
        if (shotVisualEffect != null)
        {
            CreateVisualEffect();
        }

        _recoil.recoil();
        if (shootAudio != null)
        {
            _audioSource?.PlayOneShot(shootAudio);
        }
    }

    public void PrepareToShoot()
    {
        _isShooting = true;
    }
    
    public void PrepareToStop()
    {
        _isShooting = false;
    }
}