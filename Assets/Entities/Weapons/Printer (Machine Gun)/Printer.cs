using UnityEngine;

public class Printer : RangedWeapon
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        shotDelay -= Time.deltaTime;
        if (isShooting)
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