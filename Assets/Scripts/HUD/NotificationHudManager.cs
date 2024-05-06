using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotificationHudManager : MonoBehaviour
{
    [Header("Alert Dialog")]
    [SerializeField]
    private CanvasGroup _alertDialog;
    [SerializeField]
    private TMP_Text _messageText;
    [SerializeField]
    private Button _applyButton;
    [SerializeField]
    private Button _cancleButton;

    private void Awake()
    {
        MessagingCenter.Subscribe<SaveHudManager, UnityAction>(this, SaveHudManager.MessageWantToSave, (sender, action) =>
        {
            ActivateAlert("You can save stage unlocking only, you can't save your stage progress. Are you sure to save?", action);
        });

        MessagingCenter.Subscribe<SaveHudManager, UnityAction>(this, SaveHudManager.MessageWantToLoad, (sender, action) =>
        {
            ActivateAlert("Are you sure to load this save? Your progress will be saved in autosave tempolary.", action);
        });

        MessagingCenter.Subscribe<SaveHudManager, UnityAction>(this, SaveHudManager.MessageWantToGoHikari, (sender, action) =>
        {
            ActivateAlert("Are you sure to go back to Hikari? Your progress will be lost.", action);
        });

        MessagingCenter.Subscribe<SaveHudManager, UnityAction>(this, SaveHudManager.MessageWantToGoMenu, (sender, action) =>
        {
            ActivateAlert("Are you sure to go back to title? Your progress will be lost.", action);
        });

    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<SaveHudManager, UnityAction>(this, SaveHudManager.MessageWantToSave);
        MessagingCenter.Unsubscribe<SaveHudManager, UnityAction>(this, SaveHudManager.MessageWantToLoad);
        MessagingCenter.Unsubscribe<SaveHudManager, UnityAction>(this, SaveHudManager.MessageWantToGoHikari);
        MessagingCenter.Unsubscribe<SaveHudManager, UnityAction>(this, SaveHudManager.MessageWantToGoMenu);
    }

    private void Start()
    {
        _cancleButton.onClick.AddListener(DeactiveAlert);
    }

    private void ActivateAlert(string message, UnityAction callback)
    {
        _messageText.text = message;
        _applyButton.onClick.RemoveAllListeners();
        _applyButton.onClick.AddListener(callback);
        _applyButton.onClick.AddListener(DeactiveAlert);

        _alertDialog.alpha = 1;
        _alertDialog.blocksRaycasts = true;
    }

    private void DeactiveAlert()
    {
        _alertDialog.alpha = 0;
        _alertDialog.blocksRaycasts = false;
    }
}
