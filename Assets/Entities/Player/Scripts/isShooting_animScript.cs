using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isShooting_animScript : MonoBehaviour
{
    /// <summary>  
    /// The logic for player character animation.
    /// /// - Joshua  
    /// </summary>  
  
    /// <remarks>
    /// In the future, this script should synchronize with the player character's state machine.
    /// 
    /// Changes to make:
    /// - Create one method that sets all the animation booleans.
    ///  
    /// - Joshua  
    /// </remarks>
    
    public Animator animator;
    public bool isStapler = false; 
    public bool isTac = false;

    void Start()
    {
        animator.SetBool("isShooting", false);
        animator.SetBool("isStapler", false);
        animator.SetBool("isTac", false);
    }

    public void EquipTac()
    {
        isTac = true;
        isStapler = false; 
        animator.SetBool("isStapler", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isTac", true);
    }
    public void EquipStapler()
    {
        isTac = false;
        isStapler = true; 
        animator.SetBool("isStapler", true);
        animator.SetBool("isShooting", false);
        animator.SetBool("isTac", false);
    }
    public void UnequipTac()
    {
        isTac = false;
        isStapler = false;
        animator.SetBool("isStapler", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isTac", false);
    }
    public void UnequipStapler()
    {
        isTac = false;
        isStapler = false;
        animator.SetBool("isStapler", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isTac", false);
    }
    public void TriggerShootAnimation()
    {
        // Check if either stapler or tac is equipped before shooting
        if ((isStapler || isTac) && Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(WaitForAnimationToPlay());
        }
    }
    IEnumerator WaitForAnimationToPlay()
    {
        animator.SetBool("isShooting", true);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("isShooting", false);
    }
}