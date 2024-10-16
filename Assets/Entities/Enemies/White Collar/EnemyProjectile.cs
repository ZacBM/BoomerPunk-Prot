using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] private float lifetime = 10.0f;

    void Start()
    {
        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<HPComponent>().ChangeHealth(-damage);
        }
    }
}
