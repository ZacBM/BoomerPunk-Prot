using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    /// <remarks>
    /// This script currently fulfills a function that could be utilized by other entities, namely hitboxes.
    /// For next steps, this script should get removed, & be replaced by the "HitComponent" script, which should be
    /// used anywhere a hitbox is desired.
    /// </remarks>
    
    [SerializeField] private int damage;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<HPComponent>().ChangeHealth(-damage);
        }
    }
}
