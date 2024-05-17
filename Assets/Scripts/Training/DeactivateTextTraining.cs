using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateTextTraining1 : MonoBehaviour
{
    [SerializeField] private GameObject baseAttackText;
    
    public void DeactivateBaseAttackText()
    {
        baseAttackText.SetActive(false);
    }
}
