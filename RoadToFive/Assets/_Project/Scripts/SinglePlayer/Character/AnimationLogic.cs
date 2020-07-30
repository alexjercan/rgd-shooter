using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationLogic : MonoBehaviour
{
    public Animator animator;

    public void Idle()
    {
        animator.SetBool(0, false);
        animator.SetBool(1, false);
        animator.SetBool(2, false);
        animator.SetBool(3, false);
        animator.SetBool(4, false);
        animator.SetBool(5, false);
        animator.SetBool(6, false);
    }

    public void Walk()
    {
        animator.SetBool("Walk", true);
        animator.SetBool("Backwards", false);
        animator.SetBool("Run", false);
    }

    public void Run()
    {
        animator.SetBool("Walk", true);
        animator.SetBool("Backwards", false);
        animator.SetBool("Run", true);
    }

    public void WalkBack()
    {
        animator.SetBool("Walk", true);
        animator.SetBool("Backwards", true);
        animator.SetBool("Run", false);
    }

    public void RunBack()
    {
        animator.SetBool("Walk", true);
        animator.SetBool("Backwards", true);
        animator.SetBool("Run", true);
    }

    public void LeftStrafe()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Backwards", false);
        animator.SetBool("Run", false);
        animator.SetBool("Strafe_Left", true);
        animator.SetBool("Strafe_Right", false);
    }

    public void RightStrafe()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Backwards", false);
        animator.SetBool("Run", false);
        animator.SetBool("Strafe_Left", false);
        animator.SetBool("Strafe_Right", true);
    }

    public void Jump()
    {
        animator.SetBool("Jump", true);
    }

    public void Firing()
    {
        animator.SetBool("Fire", true);
    }
}
