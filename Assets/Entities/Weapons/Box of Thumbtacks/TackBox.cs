using UnityEngine;

public class TackBox : RangedWeapon
{
    [SerializeField] private int pelletCount = 10;
    [SerializeField] private float spreadAngleDegrees = 15.0f;

    private void Update()
    {
        if (_isShooting)
        {
            _isShooting = false;
            Shoot();
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        for (int i = 0; i < pelletCount; i++)
        {
            FirePellet();
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
    }

    private Vector3 GetRandomSpread()
    {
        float randomX = Random.Range(-spreadAngleDegrees, spreadAngleDegrees);
        float randomY = Random.Range(-spreadAngleDegrees, spreadAngleDegrees);

        Vector3 spread = new Vector3(randomX, randomY, 0);

        return bulletOrigin.TransformDirection(spread.normalized);
    }
}