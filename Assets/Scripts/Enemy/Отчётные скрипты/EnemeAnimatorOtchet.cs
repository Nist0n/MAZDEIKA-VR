using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeAnimatorOtchet : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int _attack = Animator.StringToHash("Attack");
    private static readonly int _run = Animator.StringToHash("IsRunning");
    private static readonly int _walk = Animator.StringToHash("IsWalking");

    public void PlayAttack()
    {
        animator.SetTrigger(_attack);
    }

    public void IsRunning(bool condition) 
    {
        animator.SetBool(_run, condition);
    }

    public void IsWalking(bool condition)
    {
        animator.SetBool(_walk, condition);
    }
}
