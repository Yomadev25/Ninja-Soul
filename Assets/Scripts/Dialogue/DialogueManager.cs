using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    public const string MessageOnActivatedDialogue = "Activated Dialogue";
    public const string MessageOnDisplayMessage = "Display Message";
    public const string MessageOnDialogueEnded = "Dialogue Ended";

    private Queue<string> _sentences = new Queue<string>();
    private Dialogue _currentDialogue;

    private void Awake()
    {
        MessagingCenter.Subscribe<DialogueHudManager>(this, DialogueHudManager.MessageWantToDisplayNext, (sender) =>
        {
            DisplayNextSentence();
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<DialogueHudManager>(this, DialogueHudManager.MessageWantToDisplayNext);
    }

    public void ActivateDialogue(Dialogue dialogue)
    {
        _currentDialogue = dialogue;
        _sentences.Clear();
        foreach (string sentence in dialogue.messages)
        {
            _sentences.Enqueue(sentence);
        }

        MessagingCenter.Send(this, MessageOnActivatedDialogue, dialogue);
        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = _sentences.Dequeue();
        MessagingCenter.Send(this, MessageOnDisplayMessage, sentence);
    }

    private void EndDialogue()
    {
        MessagingCenter.Send(this, MessageOnDialogueEnded, _currentDialogue);
        _currentDialogue = null;
    }
}
