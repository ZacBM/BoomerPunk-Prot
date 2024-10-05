using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isShooting_animScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Animator animator;

    void Start()
    {
        animator.SetBool("isShooting", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(WaitForAnimationToPlay());

            animator.SetBool("isShooting", true);

        }
    }
    IEnumerator WaitForAnimationToPlay()
    {
        yield return new WaitForSeconds(0.001f);
        animator.SetBool("isShooting", false);

    }
}