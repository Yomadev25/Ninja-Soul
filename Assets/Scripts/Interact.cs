using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    public enum InteractType
    {
        PressButton,
        Instant
    }

    [SerializeField]
    private InteractType _interactType;
    [SerializeField]
    private UnityEvent _onInteract;

    private IInteract _target;
    private bool _canInteract;

    private void Start()
    {
        _target = GetComponent<IInteract>();
        if (_target == null)
        {
            Debug.LogErrorFormat("target is null");
        }
    }

    private void Update()
    {       
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractObject();
        }
    }

    public void InteractObject()
    {      
        if (!_canInteract || _interactType != InteractType.PressButton) return;
        if (_target != null)
            _target.Interact();
        _onInteract?.Invoke();
        _canInteract = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_interactType == InteractType.PressButton)
            {
                if (_target != null)
                    _target.EnableInteract();
                _canInteract = true;
            }
            else if (_interactType == InteractType.Instant)
            {
                _target.Interact();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {   
        if (other.CompareTag("Player"))
        {
            if (_target != null)
                _target.DisableInteract();
            _canInteract = false;
        }
    }
}
