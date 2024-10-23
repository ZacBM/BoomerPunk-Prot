using UnityEngine;

public class Hitmarker : MonoBehaviour
{
    /// <summary>
    /// Hitmarker logic. Activates a UI graphic when the player hits an enemy.
    ///
    /// - Joshua
    /// </summary>
  
    /// <remarks>
    /// Changes to make:
    /// - Fix to limit range & not trigger when there's no ammo.
    /// 
    /// - Joshua  
    /// </remarks>
    
    //Setting up variables for the hitmarker here.
    public GameObject hitmarker;
    public float distance;


    // Start is called before the first frame update
    void Start()
    {
        hitmarker.SetActive(false);
    }

    // Update is called once per frame
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

    //Activate hitmarker in scene, and turning it off.
    private void HitActive()
    {
        hitmarker.SetActive(true);
    }

    private void HitDisable()
    {
        hitmarker.SetActive(false);
    }
}
