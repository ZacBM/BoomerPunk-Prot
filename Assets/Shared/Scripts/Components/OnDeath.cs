using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class OnDeath : MonoBehaviour
{
    Rigidbody rb;
    GameObject player;
    [SerializeField] float force;
    [SerializeField] float timeToDie;

    [Header("Enemy Death Sounds")]
    public AudioClip[] deathSounds;



    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    public void Die()
    {
        if (CompareTag("Enemy"))
        {
            Knockback();
            Invoke("DestroySelf", timeToDie);
            Debug.Log("---------------");
        }

        else if (CompareTag("Player"))
        {
            PlayerDeathActions();
        }

    }

    void PlayerDeathActions()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Knockback()
    {
        try
        {
            PlayDeathSound();
            GetComponent<NavMeshAgent>().enabled = false;
            rb.isKinematic = false;

            Vector3 forceDirection = (transform.position - player.transform.position).normalized;

            rb.AddForce(forceDirection * force, ForceMode.Impulse);
            rb.AddForce(Vector3.up * force * 0.2f, ForceMode.Impulse);
        }
        catch
        {
            Destroy(gameObject);
        }
    }

    public void PlayDeathSound()
    {
        if (deathSounds.Length > 0)
        {
            if (CompareTag("Enemy"))
            {
                int randomIndex = Random.Range(0, deathSounds.Length);
                AudioSource.PlayClipAtPoint(deathSounds[randomIndex], transform.position);
            }

            else if (CompareTag("Player"))
                return;
        }
    }



    void DestroySelf()
    {
        Destroy(gameObject);
    }

}