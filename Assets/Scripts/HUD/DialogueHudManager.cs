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
    [SerializeField]
    private Button _nextButton;

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
        });

        MessagingCenter.Subscribe<DialogueManager>(this, DialogueManager.MessageOnDialogueEnded, (sender) =>
        {
            _canvasGroup.LeanAlpha(0, 0.5f);
            _isActivated = false;
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<DialogueManager, Dialogue>(this, DialogueManager.MessageOnActivatedDialogue);
        MessagingCenter.Unsubscribe<DialogueManager, string>(this, DialogueManager.MessageOnDisplayMessage);
        MessagingCenter.Unsubscribe<DialogueManager>(this, DialogueManager.MessageOnDialogueEnded);
    }

    private void Start()
    {
        _nextButton.onClick.AddListener(() => MessagingCenter.Send(this, MessageWantToDisplayNext));
    }

    private void Update()
    {
        if (_isActivated)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                MessagingCenter.Send(this, MessageWantToDisplayNext);
            }
        }
    }
}
