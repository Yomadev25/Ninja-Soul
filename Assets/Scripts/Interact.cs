using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    public const string MessageOnActiveInteract = "On Active Interact";
    public const string MessageOnDisableInteract = "On Disable Interact";

    public enum InteractType
    {
        PressButton,
        Instant
    }

    [SerializeField]
    private InteractType _interactType;
    public bool hideFromMinimap;
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

        MessagingCenter.Send(this, MessageOnDisableInteract);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_interactType == InteractType.PressButton)
            {
                if (_target != null)
                {
                    _target.EnableInteract();                    
                }

                MessagingCenter.Send(this, MessageOnActiveInteract);
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
            {
                _target.DisableInteract();              
            }

            MessagingCenter.Send(this, MessageOnDisableInteract);
            _canInteract = false;
        }
    }
}
