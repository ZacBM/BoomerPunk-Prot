using UnityEngine;

public class Hitmarker : MonoBehaviour
{
    public GameObject hitmarker;
    public float distance;

    void Start()
    {
        hitmarker.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distance))
        {
            if (hit.collider.tag == "Enemy")
            {
                HitActive();
                Invoke("HitDisable", 0.2f);
            }
        }
    }

    private void HitActive()
    {
        hitmarker.SetActive(true);
    }

    private void HitDisable()
    {
        hitmarker.SetActive(false);
    }
}
