using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(AmmoComponent))]
[RequireComponent(typeof(HitComponent))]
[RequireComponent(typeof(RigidbodyHelperComponent))]
[RequireComponent(typeof(SpawnComponent))]

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]

public class TackBox : MonoBehaviour, RangedWeapon
{
    /// <summary>
    /// The tackbox weapon.
    ///
    /// Imitates a shotgun.
    ///
    /// - Joshua
    /// </summary>
  
    /// <remarks>
    /// I would like to further abstract the weapon code to make it D.R.Y.er, but I'm not quite sure how to.
    /// 
    /// - Joshua  
    /// </remarks>
    
    // Custom components.
    private AmmoComponent ammoHolder;
    private HitComponent hitbox;
    private RigidbodyHelperComponent rigidbodyHelper;

    [SerializeField] public Transform bulletOrigin;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected float shotRange = 10.0f;
    [SerializeField] protected float bulletForce = 50.0f;

    [SerializeField] protected int pelletCount = 10; // Number of pellets fired
    [SerializeField] protected float spreadAngle = 15.0f; // Spread angle in degrees

    [SerializeField] protected VisualEffect shotVisualEffect;

    public AudioSource audioSource;
    public AudioClip shootAudio;
    public AudioClip pickupAudio;

    protected Recoil recoil;

    private bool isShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize components.
        ammoHolder = GetComponent<AmmoComponent>();
        hitbox = GetComponent<HitComponent>();
        rigidbodyHelper = GetComponent<RigidbodyHelperComponent>();

        hitbox.isActive = false;

        rigidbodyHelper.ActivateRigidbody();
        //recoil = GetComponent<Recoil>();
        recoil = FindObjectOfType<Recoil>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Ensure AudioSource exists
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            isShooting = false;
            Shoot();
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

    public void Shoot()
    {
        if (ammoHolder.IsEmpty())
        {
            Debug.Log("This tackbox is out of ammo!");
            return;
        }

        ammoHolder.UseAmmo();

        for (int i = 0; i < pelletCount; i++)
        {
            FirePellet();
        }


        // Trigger visual effects and audio
        if (shotVisualEffect != null)
        {
            CreateVisualEffect();
        }

        recoil?.recoil();
        if (shootAudio != null)
        {
            audioSource?.PlayOneShot(shootAudio);
        }
    }

    void FirePellet()
    {
        Vector3 spread = GetRandomSpread();

        RaycastHit hit;
        Vector3 direction = bulletOrigin.forward + spread; 
        
        Debug.Log($"Pellet fired with spread: {spread}, direction: {direction}");

        if (Physics.Raycast(bulletOrigin.position, direction, out hit, shotRange))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.TryGetComponent<HPComponent>(out HPComponent hpc))
                {
                    hpc.ChangeHealth(-1); // Apply damage to the hit object
                }
            }
        }

        Debug.DrawRay(bulletOrigin.position, direction * shotRange, Color.red, 1.0f);
        InstantiateBullet(direction);
    }

    Vector3 GetRandomSpread()
    {
        float randomX = Random.Range(-spreadAngle, spreadAngle);
        float randomY = Random.Range(-spreadAngle, spreadAngle);

        Vector3 spread = new Vector3(randomX, randomY, 0);

        return bulletOrigin.TransformDirection(spread.normalized);
    }

    void CreateVisualEffect()
    {
        shotVisualEffect.Play();
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
            audioSource.PlayOneShot(pickupAudio);
        }
    }

    public void Throw(Vector3 throwDirection)
    {
        transform.SetParent(null);
        rigidbodyHelper.ActivateRigidbody();
        GetComponent<Rigidbody>().AddForce(throwDirection, ForceMode.Impulse);
    }

    public void InstantiateBullet(Vector3 direction)
    {
        if (bullet != null)
        {
            GameObject spawnedBullet = Instantiate(bullet, bulletOrigin.position, Quaternion.LookRotation(direction));

            // Apply force to the bullet in the correct direction
            Rigidbody bulletRigidbody = spawnedBullet?.GetComponent<Rigidbody>();
            bulletRigidbody?.AddForce(direction * bulletForce, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(bulletOrigin.position, shotRange);
    }
}
