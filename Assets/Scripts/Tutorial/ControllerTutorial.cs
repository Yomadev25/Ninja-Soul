using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerTutorial : MonoBehaviour
{
    public const string MessageOnActivateDestination = "Activate Destination";
    public const string MessageOnTutorialComplete = "On Tutorial Complete";

    [SerializeField]
    private Event _event;
    [SerializeField]
    private Transform[] _destinations;
    [SerializeField]
    private Transform _playerPos;

    [Header("Input Setting")]
    [SerializeField]
    private InputActionReference _movementInput;
    [SerializeField]
    private InputActionReference _sprintInput;
    [SerializeField]
    private InputActionReference _dashInput;

    [Header("HUD")]
    [SerializeField]
    private TMP_Text _moveText;
    [SerializeField]
    private TMP_Text _sprintText;
    [SerializeField]
    private TMP_Text _dashText;

    private int _currentDestination;
    private bool _isActivated;
    private bool _isCompleted;

    private bool _isMoved;
    private bool _isSprinted;
    private bool _isDashed;

    private void Start()
    {
        _playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        _movementInput.action.started += (ctx) =>
        {
            _isMoved = true;
            _moveText.color = Color.green;
            ActivateEvent();
        };

        _sprintInput.action.started += (ctx) =>
        {
            _isSprinted = true;
            _sprintText.color = Color.green;
            ActivateEvent();
        };

        _dashInput.action.started += (ctx) =>
        {
            _isDashed = true;
            _dashText.color = Color.green;
            ActivateEvent();
        };
    }

    private void ActivateEvent()
    {
        if (!_isMoved || !_isSprinted || !_isDashed) return;
        if (_isActivated) return;
        if (_isCompleted) return;

        _isActivated = true;
        EventManager.Instance.ActivatedEvent(_event);
        UpdateDestination();
    }

    private void Update()
    {
        if (!_isActivated) return;
        if (Vector3.Distance(_playerPos.position, _destinations[_currentDestination].position) < 1f)
        {
            _currentDestination++;
            if (_currentDestination >= _destinations.Length)
            {
                Complete();
                return;
            }

            UpdateDestination();
        }
    }

    private void UpdateDestination()
    {
        foreach (Transform destination in _destinations)
        {
            destination.gameObject.SetActive(false);
        }

        _destinations[_currentDestination].gameObject.SetActive(true);
        MessagingCenter.Send(this, MessageOnActivateDestination, _destinations[_currentDestination].transform);
    }

    private void Complete()
    {
        _isActivated = false;
        _isCompleted = true;

        foreach (Transform destination in _destinations)
        {
            destination.gameObject.SetActive(false);
        }

        EventManager.Instance.ArchieveEvent(_event);
        MessagingCenter.Send(this, MessageOnTutorialComplete);
    }
}
