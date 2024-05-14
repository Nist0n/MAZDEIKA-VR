using System;
using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [SerializeField] private NPCConversation startingDialog;

    private void Start()
    {
        StartDialog(startingDialog);
    }

    private void StartDialog(NPCConversation dialog)
    {
        ConversationManager.Instance.StartConversation(dialog);
    }
}
