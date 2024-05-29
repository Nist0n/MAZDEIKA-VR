using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAchievement : MonoBehaviour
{
    public void StartAnimAchievement()
    {
        LeanTween.scale(gameObject, new Vector3(1.5f, 1.5f, 1.5f), 2f).setDelay(0.1f)
            .setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(gameObject, new Vector3(0f, 0f, 0f), 2f).setDelay(3f)
            .setEase(LeanTweenType.easeOutElastic);
    }
}
