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
        
    }

    private void OnDestroy()
    {
        
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
