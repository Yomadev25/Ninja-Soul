using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    private GameObject _pointer;

    private Transform _target;
    private Vector3 _rotation;

    private void Awake()
    {
        MessagingCenter.Subscribe<ControllerTutorial, Transform>(this, ControllerTutorial.MessageOnActivateDestination, (sender, target) =>
        {
            _target = target;
            _pointer.SetActive(true);
        });

        MessagingCenter.Subscribe<ControllerTutorial>(this, ControllerTutorial.MessageOnTutorialComplete, (sender) =>
        {
            _pointer.SetActive(false);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<ControllerTutorial, Transform>(this, ControllerTutorial.MessageOnActivateDestination);
        MessagingCenter.Unsubscribe<ControllerTutorial>(this, ControllerTutorial.MessageOnTutorialComplete);
    }

    private void Update()
    {
        if (_target != null)
        {
            transform.LookAt(_target);
            _rotation = transform.eulerAngles;
            _rotation.x = 0;
            _rotation.z = 0;
            transform.eulerAngles = _rotation;
        }
    }
}
