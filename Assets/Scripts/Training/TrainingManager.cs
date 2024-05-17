using System;
using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using TMPro;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [SerializeField] private NPCConversation startingDialog;
    [SerializeField] private NPCConversation secondDialog;
    [SerializeField] private TextMeshProUGUI baseAttackTrainingText;
    [SerializeField] private Animator textAnim;
    [SerializeField] private Animator baseAttackVideoAnimator;
    [SerializeField] private Animator shieldVideoAnimator;
    [SerializeField] private Animator breakShieldVideoAnimator;

    public bool FirstSkillTraining = false;
    public bool SecondSkillTraining = false;
    public bool ThirdSkillTraining = false;
    public int BaseAttackCompletedTimes;

    private bool _training1Completed = false;

    private void Start()
    {
        StartDialog(startingDialog);
    }

    private void Update()
    {
        BaseAttackCompletedTimes = Mathf.Clamp(BaseAttackCompletedTimes, 0, 5);
        if (!_training1Completed) CheckProgressCompletionBaseAttackTraining();
    }

    private void StartDialog(NPCConversation dialog)
    {
        ConversationManager.Instance.StartConversation(dialog);
    }

    private void CheckProgressCompletionBaseAttackTraining()
    {
        baseAttackTrainingText.color = Color.red;
        baseAttackTrainingText.text = $"Успешно совершено атак: {BaseAttackCompletedTimes}/5";
        if (BaseAttackCompletedTimes >= 5)
        {
            baseAttackTrainingText.color = Color.green;
            _training1Completed = true;
            textAnim.SetTrigger("hideText");
            baseAttackVideoAnimator.SetTrigger("hideVideo");
            StartDialog(secondDialog);
        }
    }

    public void StartTraining1()
    {
        baseAttackTrainingText.gameObject.SetActive(true);
        FirstSkillTraining = true;
    }
}
