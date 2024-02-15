using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Resume(InputAction.CallbackContext ctx)
    {
        if (_canvasGroup.alpha != 1) return;
        MessagingCenter.Send(this, MessageWantToResume);
    }

    private void OnDestroy()
    {
        _pauseInput.action.started -= Resume;
    }
}
