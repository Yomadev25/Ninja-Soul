using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractHud : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private TMP_Text _actionText;
    [SerializeField]
    private Image _circle;

    private void Awake()
    {
        MessagingCenter.Subscribe<Interact>(this, Interact.MessageOnActiveInteract, (sender) =>
        {
            _actionText.text = sender.actionName;
            EnableInteract();
        });

        MessagingCenter.Subscribe<Interact>(this, Interact.MessageOnDisableInteract, (sender) =>
        {
            DisableInteract();
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<Interact>(this, Interact.MessageOnActiveInteract);
        MessagingCenter.Unsubscribe<Interact>(this, Interact.MessageOnDisableInteract);
    }

    private void EnableInteract()
    {
        _canvasGroup.LeanAlpha(1f, 0.2f).setOnUpdate(x =>
        {
            _circle.fillAmount = x;
        });
    }

    private void DisableInteract()
    {
        _canvasGroup.LeanAlpha(0f, 0.2f);
    }
}
