using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;
using static GestureManagerVR.SampleDisplay;
using static Mivry;
using UnityEngine.XR.Interaction.Toolkit;

public class GestureEvents : MonoBehaviour
{
    private Skills _skills;
    private GestureRecognition gr;
    private GestureCombinations gc;
    private TrainingSkills _trainingSkills;
    private TrainingManager _trainingManager;
    private List<string> stroke = new List<string>();
    private int stroke_index = 0;
    private GameObject active_controller_pointer = null;
    private GameObject active_controller = null;

    private void Start()
    {
        _trainingManager = FindObjectOfType<TrainingManager>();
        _trainingSkills = FindObjectOfType<TrainingSkills>();
        _skills = FindObjectOfType<Skills>();
    }
    private void Update()
    {
        float trigger_right = getInputControlValue("<XRController>{RightHand}/trigger");

        if (trigger_right > 0.85)
        {
            active_controller = GameObject.Find("Right Hand");
            active_controller_pointer = GameObject.FindGameObjectWithTag("Right Pointer");
            addToStrokeTrail(active_controller_pointer.transform.position);
            return;
        }

        if (stroke.Count > 0)
        {
            foreach (var star in stroke)
            {
                Destroy(GameObject.Find(star));
                stroke_index = 0;
            }
            stroke.Clear();
        }
    }
    public void OnGestureCompleted(GestureCompletionData gestureCompletionData)
    {
        Debug.Log(gestureCompletionData.gestureName);
        Debug.Log(gestureCompletionData.similarity);

        if (gestureCompletionData.gestureID < 0) 
        {
            return;
        }

        if (gestureCompletionData.gestureName == "BaseAttack")
        {
            if(gestureCompletionData.similarity >= 0.4f)
            {
                if (_trainingSkills != null && !_trainingManager.TrainingIsOver)
                {
                    _trainingSkills.BaseAttack();
                }
                else _skills.BaseAttack();
            }
        }
        
        if (gestureCompletionData.gestureName == "shield")
        {
            if (gestureCompletionData.similarity >= 0.54f)
            {
                if (_trainingSkills != null && !_trainingManager.TrainingIsOver)
                {
                    _trainingSkills.ActivateShield();
                }
                else _skills.ActivateShield();
            }
        }

        if (gestureCompletionData.gestureName == "BreakShieldAttack")
        {
            if (gestureCompletionData.similarity >= 0.5f)
            {
                if (_trainingSkills != null && !_trainingManager.TrainingIsOver) _trainingSkills.BreakShield();
                else _skills.BreakShield();
            }
        }
        
        if (gestureCompletionData.gestureName == "defence")
        {
            if (SaveSystem.instance.secondEnemyDefeated)
            {
                if (gestureCompletionData.similarity >= 0.4f) _skills.DefenceSkill();
            }
        }
        
        if (gestureCompletionData.gestureName == "ignite")
        {
            if (SaveSystem.instance.firstEnemyDefeated)
            {
                if (gestureCompletionData.similarity >= 0.5f) _skills.IgniteSkill();
            }
        }
        
        if (gestureCompletionData.gestureName == "Heal")
        {
            if (SaveSystem.instance.thirdEnemyDefeated)
            {
                if (gestureCompletionData.similarity >= 0.4f) _skills.HealSkill();
            }
        }
        
        if (gestureCompletionData.gestureName == "StunnedAttack")
        {
            if (SaveSystem.instance.firstEnemyDefeated)
            {
                if (gestureCompletionData.similarity >= 0.5f) _skills.StunningAttack();
            }
        }
        
        if (gestureCompletionData.gestureName == "clear")
        {
            if (SaveSystem.instance.secondEnemyDefeated)
            {
                if (gestureCompletionData.similarity >= 0.4f) _skills.CleanSkill();
            }
        }
        
        if (gestureCompletionData.gestureName == "increasedamage")
        {
            if (SaveSystem.instance.thirdEnemyDefeated)
            {
                if (gestureCompletionData.similarity >= 0.4f) _skills.IncreaseDamageSkill();
            }
        }
    }

    public void addToStrokeTrail(Vector3 p)
    {
        GameObject star_instance = Instantiate(GameObject.Find("star"), p, Quaternion.identity, active_controller.transform);
        GameObject star = new GameObject("stroke_" + stroke_index++);
        star_instance.name = star.name + "_instance";
        star_instance.transform.SetParent(star.transform, false);
        System.Random random = new System.Random();
        star.transform.position = new Vector3((float)random.NextDouble() / 80, (float)random.NextDouble() / 80, (float)random.NextDouble() / 80) + p;
        star.transform.rotation = new Quaternion((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f).normalized;
        stroke.Add(star.name);
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
