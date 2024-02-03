using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HudEvent : MonoBehaviour
{
    [SerializeField]
    private EventSystem _eventSystem;

    [Header("Weapon Dialog")]
    [SerializeField]
    private GameObject _weaponButton;

    private void Start()
    {
        _eventSystem.firstSelectedGameObject = _weaponButton.gameObject;
    }
}
