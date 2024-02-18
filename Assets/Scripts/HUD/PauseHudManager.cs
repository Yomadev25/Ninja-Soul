using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseHudManager : MonoBehaviour
{
    public const string MessageWantToResume = "Want To Resume";

    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private InputActionReference _pauseInput;
    
    private void Start()
    {
        _pauseInput.action.started += Resume;
    }

    private void OnDestroy()
    {
        _pauseInput.action.started -= Resume;
    }

    private void Resume(InputAction.CallbackContext ctx)
    {
        if (_canvasGroup.alpha != 1) return;
        MessagingCenter.Send(this, MessageWantToResume);
    }
}
