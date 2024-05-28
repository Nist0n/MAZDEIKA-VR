using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HomeLocationStart : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private XRInteractorLineVisual _XRInteractorLineVisual;

    void Start()
    {
        _XRInteractorLineVisual.enabled = true;
        animator.SetTrigger("fadeOut");
    }
}
