using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoulTutorial : MonoBehaviour
{
    public const string MessageOnTutorialComplete = "On Tutorial Complete";

    [SerializeField]
    private InputActionReference _soulInput;
    [SerializeField]
    private Event _event;
    [SerializeField]
    private GameObject[] _enemies;
    [SerializeField]
    private TMP_Text _soulText;

    private bool _isActivated;

    private void Awake()
    {
        MessagingCenter.Subscribe<PlayerManager>(this, PlayerManager.MessageOnSoulChanged, (sender) =>
        {
            if (sender.soulBerserk)
            {
                ActivateEvent();
            }
        });

        MessagingCenter.Subscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent, (sender, @event) =>
        {
            if (@event == _event)
            {
                MessagingCenter.Send(this, MessageOnTutorialComplete);
            }
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<PlayerManager>(this, PlayerManager.MessageOnSoulChanged);
        MessagingCenter.Unsubscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent);
    }

    private void ActivateEvent()
    {
        if (_isActivated) return;

        _isActivated = true;
        EventManager.Instance.ActivateEvent(_event);

        foreach (GameObject enemy in _enemies)
        {
            enemy.SetActive(true);
        }
    }
}
