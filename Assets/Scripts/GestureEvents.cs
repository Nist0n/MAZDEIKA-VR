using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureEvents : MonoBehaviour
{
    GestureRecognition gesture = new GestureRecognition();

    private void Start()
    {
        //gesture.contdIdentify(GestureRecognition.UpdateHeadPositionPolicy.UseInitial, GestureRecognition.UpdateHeadPositionPolicy.UseInitial, ref );
    }

    private void Update()
    {
        
    }

    public void OnGestureCompleted(GestureCompletionData gestureCompletionData)
    {

        if (gestureCompletionData.gestureID < 0) 
        {
            Debug.Log("LOX");
            return;
        }

        if (gestureCompletionData.gestureName == "vlad")
        {
            Debug.Log("vlad");
        }

        if (gestureCompletionData.gestureName == "oleg")
        {
            Debug.Log("oleg");
        }

        
    }
}
