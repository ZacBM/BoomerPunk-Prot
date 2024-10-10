using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(AmmoComponent))]
[RequireComponent(typeof(HitComponent))]
[RequireComponent(typeof(RigidbodyHelperComponent))]
[RequireComponent(typeof(SpawnComponent))]

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]

public class Printer : MonoBehaviour, RangedWeapon
{
    // Custom components.
    private AmmoComponent ammoHolder;
    private HitComponent hitbox;
    private RigidbodyHelperComponent rigidbodyHelper;
    
    [SerializeField] public Transform bulletOrigin;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected float shotRange = 10.0f;
    [SerializeField] protected float bulletForce = 50.0f;
    
    [SerializeField] protected VisualEffect shotVisualEffect;

    private AudioSource audioSource;
    public AudioClip shootAudio;
    public AudioClip pickupAudio;

    protected Recoil recoil;
    
    void Start()
    {
        // Initialize components.
        ammoHolder = GetComponent<AmmoComponent>();
        hitbox = GetComponent<HitComponent>();
        rigidbodyHelper = GetComponent<RigidbodyHelperComponent>();
        
        hitbox.isActive = false;
        
        audioSource = GetComponent<AudioSource>();
        
        rigidbodyHelper.ActivateRigidbody();
        //recoil = GetComponent<Recoil>();
        recoil = FindObjectOfType<Recoil>();
    }

    public void Shoot()
    {
        Debug.Log("This printer wants to give someone a paper cut!");
        if (ammoHolder.IsEmpty())
        {
            Debug.Log("This printer's out of ammo!");
            return;
        }
        ammoHolder.UseAmmo();
        RaycastHit hit;
        Physics.Raycast(bulletOrigin.position, bulletOrigin.forward, out hit, shotRange);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.TryGetComponent<HPComponent>(out HPComponent hpComponent))
            {
                hpComponent.ChangeHealth(-1);
            }
            Debug.Log("This printer gave" + hit.collider.gameObject.name + "a paper cut!");
        }
        //Debug.DrawRay(bulletOrigin.position, bulletOrigin.forward * shotRange, Color.red, 1.0f);
        InstantiateBullet();
        if (shotVisualEffect != null) CreateVisualEffect();

        recoil?.recoil();
        if (shootAudio != null) audioSource?.PlayOneShot(shootAudio);
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

        if (pickupAudio != null) audioSource.PlayOneShot(pickupAudio);

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
        shotVisualEffect.Play();
    }
    
    public void InstantiateBullet()
    {
        if (bullet != null)
        {
            GameObject spawnedBullet = Instantiate(bullet, bulletOrigin.position, bulletOrigin.rotation);

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
}