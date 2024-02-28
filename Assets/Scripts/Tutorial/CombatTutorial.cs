using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatTutorial : MonoBehaviour
{
    public const string MessageOnTutorialComplete = "On Tutorial Complete";

    [SerializeField]
    private InputActionReference _combatInput;
    [SerializeField]
    private Event _scarecrowEvent;
    [SerializeField]
    private GameObject[] _scarecrows;
    [SerializeField]
    private Event _enemyEvent;
    [SerializeField]
    private GameObject[] _enemies;

    [Header("HUD")]
    [SerializeField]
    private TMP_Text _attackText;

    private bool _isActivated;

    private void Start()
    {
        _combatInput.action.started += (ctx) =>
        {
            _attackText.color = Color.green;
            ActivateEvent();
        };
    }

    private void Awake()
    {
        MessagingCenter.Subscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent, (sender, @event) =>
        {
            if (@event == _scarecrowEvent)
            {
                Invoke(nameof(ActivateEventPhaseTwo), 3f);
            }  
            else if (@event == _enemyEvent)
            {
                MessagingCenter.Send(this, MessageOnTutorialComplete);
            }
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent);
    }

    private void ActivateEvent()
    {
        if (_isActivated) return;

        _isActivated = true;
        EventManager.Instance.ActivateEvent(_scarecrowEvent);

        foreach (GameObject scarecrow in _scarecrows)
        {
            scarecrow.SetActive(true);
        }
    }

    private void ActivateEventPhaseTwo()
    {
        EventManager.Instance.ActivateEvent(_enemyEvent);

        foreach (GameObject enemy in _enemies)
        {
            enemy.SetActive(true);
        }
    }
}
