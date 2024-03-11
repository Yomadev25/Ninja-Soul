using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHudManager : MonoBehaviour
{
    public const string MessageWantToDisplayNext = "Want To Display Next";

    [SerializeField]
    private TMP_Text _nameText;
    [SerializeField]
    private TMP_Text _messageText;
    [SerializeField]
    private CanvasGroup _canvasGroup;

    private bool _isActivated;

    private void Awake()
    {
        MessagingCenter.Subscribe<DialogueManager, Dialogue>(this, DialogueManager.MessageOnActivatedDialogue, (sender, dialogue) =>
        {
            _canvasGroup.LeanAlpha(1, 0.5f);
            _nameText.text = dialogue.speaker;
            _isActivated = true;
        });

        MessagingCenter.Subscribe<DialogueManager, string>(this, DialogueManager.MessageOnDisplayMessage, (sender, message) =>
        {
            _messageText.text = message;
            LeanTween.value(0, 1, 0.2f).setOnUpdate(x =>
            {
                _messageText.color = new Color(1, 1, 1, x);
            });
        });

        MessagingCenter.Subscribe<DialogueManager, Dialogue>(this, DialogueManager.MessageOnDialogueEnded, (sender, dialogue) =>
        {
            _canvasGroup.LeanAlpha(0, 0.5f);
            _isActivated = false;
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<DialogueManager, Dialogue>(this, DialogueManager.MessageOnActivatedDialogue);
        MessagingCenter.Unsubscribe<DialogueManager, string>(this, DialogueManager.MessageOnDisplayMessage);
        MessagingCenter.Unsubscribe<DialogueManager, Dialogue>(this, DialogueManager.MessageOnDialogueEnded);
    }

    private void Update()
    {
        if (_isActivated)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                MessagingCenter.Send(this, MessageWantToDisplayNext);
            }
        }
    }
}
