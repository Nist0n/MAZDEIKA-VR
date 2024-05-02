using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureEvents : MonoBehaviour
{
    private Skills _skills;

    private void Start()
    {
        _skills = FindObjectOfType<Skills>();
    }

    private void Update()
    {
        
    }

    public void OnGestureCompleted(GestureCompletionData gestureCompletionData)
    {

        Debug.Log(gestureCompletionData.similarity);
        Debug.Log(gestureCompletionData.gestureName);

        if (gestureCompletionData.gestureID < 0) 
        {
            Debug.Log("LOX");
            return;
        }

        if (gestureCompletionData.gestureName == "BaseAttack")
        {
            if(gestureCompletionData.similarity >= 0.3f)
            {
                _skills.BaseAttack();
                Debug.Log("EEEEEEEE");
            }
        }

        if (gestureCompletionData.gestureName == "shield")
        {
            if (gestureCompletionData.similarity >= 0.3f) _skills.ActivateShield();
        }

        if (gestureCompletionData.gestureName == "BreakShieldAttack")
        {
            if (gestureCompletionData.similarity >= 0.3f) _skills.BreakShield();
        }

    }
}
