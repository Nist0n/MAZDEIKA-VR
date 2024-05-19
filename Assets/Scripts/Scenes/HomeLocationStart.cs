using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeLocationStart : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    void Start()
    {
        animator.SetTrigger("fadeOut");
    }
}
