using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureEvents : MonoBehaviour
{
    public void OnGestureCompleted(GestureCompletionData gestureCompletionData)
    {
        if (gestureCompletionData.gestureID < 0) 
        {
            string errorMessage = GestureRecognition.getErrorMessage(gestureCompletionData.gestureID);
            return;
        }
        if (gestureCompletionData.similarity >= 0.5)
        {
            ///
        }
    }
}
