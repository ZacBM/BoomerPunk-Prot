using UnityEngine;

public class Printer : RangedWeapon
{
    private void Update()
    {
        shotDelay -= Time.deltaTime;
        if (_isShooting)
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        if (shotDelay > 0)
        {
            return;
        }
        shotDelay = shotDelaySet;
        
        base.Shoot();
        
        RaycastHit hit;
        Physics.Raycast(bulletOrigin.position, bulletOrigin.forward, out hit, shotRange);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.TryGetComponent<HPComponent>(out HPComponent hpComponent))
            {
                hpComponent.ChangeHealth(-1);
            }
        }
    }
}