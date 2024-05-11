using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;
using static GestureManagerVR.SampleDisplay;
using static Mivry;

public class GestureEvents : MonoBehaviour
{
    private Skills _skills;
    private GestureRecognition gr;
    private GestureCombinations gc;
    private List<string> stroke = new List<string>();
    private int stroke_index = 0;
    private GameObject active_controller_pointer = null;
    private GameObject active_controller = null;

    private void Start()
    {
        _skills = FindObjectOfType<Skills>();
        Debug.Log(Application.streamingAssetsPath);
    }

    private void Update()
    {
        float trigger_right = getInputControlValue("<XRController>{RightHand}/trigger");

        if (trigger_right > 0.85)
        {
            // Right controller trigger pressed.
            active_controller = GameObject.Find("Right Hand");
            active_controller_pointer = GameObject.FindGameObjectWithTag("Right Pointer");
            addToStrokeTrail(active_controller_pointer.transform.position);
            return;
        }

        foreach (string star in stroke)
        {
            Destroy(GameObject.Find(star));
            stroke_index = 0;
        }
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

    public void addToStrokeTrail(Vector3 p)
    {
        GameObject star_instance = Instantiate(GameObject.Find("star"));
        GameObject star = new GameObject("stroke_" + stroke_index++);
        star_instance.name = star.name + "_instance";
        star_instance.transform.SetParent(star.transform, false);
        System.Random random = new System.Random();
        star.transform.position = new Vector3((float)random.NextDouble() / 80, (float)random.NextDouble() / 80, (float)random.NextDouble() / 80) + p;
        star.transform.rotation = new Quaternion((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f).normalized;
        //star.transform.rotation.Normalize();
        float star_scale = (float)random.NextDouble() + 0.3f;
        star.transform.localScale = new Vector3(star_scale, star_scale, star_scale);
        if (this.compensate_head_motion)
        {
            star.transform.SetParent(Camera.main.gameObject.transform);
        }
        stroke.Add(star.name);
    }

    public bool compensate_head_motion
    {
        get
        {
            if (gr != null)
            {
                return gr.getUpdateHeadPositionPolicy() == GestureRecognition.UpdateHeadPositionPolicy.UseLatest;
            }
            if (gc != null)
            {
                return gc.getUpdateHeadPositionPolicy(0) == GestureRecognition.UpdateHeadPositionPolicy.UseLatest;
            }
            return false;
        }
        set
        {
            GestureRecognition.UpdateHeadPositionPolicy p = value ? GestureRecognition.UpdateHeadPositionPolicy.UseLatest : GestureRecognition.UpdateHeadPositionPolicy.UseInitial;
            if (gr != null)
            {
                gr.setUpdateHeadPositionPolicy(p);
            }
            if (gc != null)
            {
                for (int part = gc.numberOfParts() - 1; part >= 0; part--)
                {
                    gc.setUpdateHeadPositionPolicy(part, p);
                }
            }
        }
    }

    public static float getInputControlValue(string controlName)
    {

        InputControl control = InputSystem.FindControl(controlName); // eg: "<XRController>{RightHand}/trigger"
        switch (control)
        {
            case AxisControl axisControl:
                return axisControl.ReadValue();
            case DoubleControl doubleControl:
                return (float)doubleControl.ReadValue();
            case IntegerControl integerControl:
                return integerControl.ReadValue();
            case QuaternionControl quaternionControl:
                Debug.LogError($"Mivry.getInputControlValue : QuaternionControl '${controlName}' not supported.");
                return 0.0f;
            case TouchControl touchControl:
                return touchControl.ReadValue().pressure;
            case TouchPhaseControl phaseControl:
                var phase = phaseControl.ReadValue();
                switch (phase)
                {
                    case UnityEngine.InputSystem.TouchPhase.Began:
                    case UnityEngine.InputSystem.TouchPhase.Stationary:
                    case UnityEngine.InputSystem.TouchPhase.Moved:
                        return 1.0f;
                    case UnityEngine.InputSystem.TouchPhase.None:
                    case UnityEngine.InputSystem.TouchPhase.Ended:
                    case UnityEngine.InputSystem.TouchPhase.Canceled:
                    default:
                        return 0.0f;
                }
            case Vector2Control vector2Control:
                return vector2Control.ReadValue().magnitude;
            case Vector3Control vector3Control:
                return vector3Control.ReadValue().magnitude;

        }
        return 0.0f;
    }
}
