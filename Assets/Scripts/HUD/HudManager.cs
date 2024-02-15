using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    [Header("Gameplay HUD")]
    [SerializeField]
    private CanvasGroup _gameplayHud;

    [Header("Pause HUD")]
    [SerializeField]
    private CanvasGroup _pauseHud;

    private void Awake()
    {
        MessagingCenter.Subscribe<GameManager>(this, GameManager.MessageWantToPause, (sender) =>
        {
            OnPause();
        });

        MessagingCenter.Subscribe<PauseHudManager>(this, PauseHudManager.MessageWantToResume, (sender) =>
        {
            OnResume();
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<GameManager>(this, GameManager.MessageWantToPause);
        MessagingCenter.Unsubscribe<PauseHudManager>(this, PauseHudManager.MessageWantToResume);
    }

    private void Start()
    {
        _gameplayHud.alpha = 1f;
        _gameplayHud.interactable = true;
        _gameplayHud.blocksRaycasts = true;

        _pauseHud.alpha = 0f;
        _pauseHud.interactable = false;
        _pauseHud.blocksRaycasts = false;
    }

    private void OnPause()
    {
        _gameplayHud.LeanAlpha(0f, 0.5f);
        _gameplayHud.interactable = false;
        _gameplayHud.blocksRaycasts = false;

        _pauseHud.LeanAlpha(1f, 0.5f);
        _pauseHud.interactable = true;
        _pauseHud.blocksRaycasts = true;
    }

    private void OnResume()
    {
        _pauseHud.LeanAlpha(0f, 0.5f);
        _pauseHud.interactable = false;
        _pauseHud.blocksRaycasts = false;

        _gameplayHud.LeanAlpha(1f, 0.5f);
        _gameplayHud.interactable = true;
        _gameplayHud.blocksRaycasts = true;
    }
}
