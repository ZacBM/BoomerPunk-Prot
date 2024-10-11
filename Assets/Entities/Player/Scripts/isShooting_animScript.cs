using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isShooting_animScript : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator.SetBool("isShooting", false);
    }

    // Update is called once per frame
    public void TriggerShootAnimation()
    {
        if (Input.GetButtonDown("Fire1")) StartCoroutine(WaitForAnimationToPlay());
    }
    IEnumerator WaitForAnimationToPlay()
    {
        animator.SetBool("isShooting", true);
        yield return new WaitForSeconds(0.001f);
        animator.SetBool("isShooting", false);
    }
}