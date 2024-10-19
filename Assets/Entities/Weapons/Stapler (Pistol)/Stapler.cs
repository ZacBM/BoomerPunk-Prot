using UnityEngine;

public class Stapler : RangedWeapon
{
    protected override void Start()
    {
        base.Start();
    }
    
    protected override void Update()
    {
        if (isShooting)
        {
            isShooting = false;
            Shoot();
        }
    }

    public void Shoot()
    {
        base.Shoot();
        
        RaycastHit hit;
        Physics.Raycast(bulletOrigin.position, bulletOrigin.forward, out hit, shotRange);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.TryGetComponent<HPComponent>(out HPComponent hpc))
            {
                hpc.ChangeHealth(-1);
            }
        }
    }
}