using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public void ActivateIsAttacking()
    {
        animator.SetBool("isAttacking", true);
    }

    public void DeActivateIsAttacking()
    {
        animator.SetBool("isAttacking", false);
    }
}
