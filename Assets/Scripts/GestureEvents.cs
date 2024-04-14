using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureEvents : MonoBehaviour
{
    public void OnGestureCompleted(GestureCompletionData gestureCompletionData)
    {
        if (gestureCompletionData.gestureID < 0) 
        {
            Debug.Log("LOX");
            return;
        }
        if (gestureCompletionData.similarity >= 0.5)
        {
            Debug.Log("gg");
        }
    }
}
