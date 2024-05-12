using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Hikari : MonoBehaviour
{
    [Header("Weapon Tutorial")]
    [SerializeField]
    private Event _weaponTutorialEvent;
    [SerializeField]
    private GameObject _weaponTutorial;
    [SerializeField]
    private InputActionReference _weaponInput;

    private void Awake()
    {
        PlayerData.Instance.SetSpawnPoint(default);

        MessagingCenter.Subscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent, (sender, @event) =>
        {
            if (@event == _weaponTutorialEvent)
            {
                _weaponTutorial.SetActive(false);
                if (PlayerData.Instance.GetPlayerData().tutorial < 2)
                {
                    PlayerData.Instance.GetPlayerData().tutorial = 2;
                }
            }
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent);
    }

    private void Start()
    {
        AudioManager.Instance.PlayBGM("Hikari");

        if (PlayerData.Instance.GetPlayerData().tutorial >= 2) return;
        Player player = PlayerData.Instance.GetPlayerData();
        int i = 0;
        if (player.genbu)
        {
            i++;
        }
        if (player.byakko)
        {
            i++;
        }
        if (player.seiryu)
        {
            i++;
        }
        if (player.suzaku)
        {
            i++;
        }

        if (i == 1)
        {
            EventManager.Instance.ActivateEvent(_weaponTutorialEvent);
            _weaponTutorial.SetActive(true);
        }
    }

    private void Update()
    {
        if (_weaponInput.action.triggered)
        {
            EventManager.Instance.ArchieveEvent(_weaponTutorialEvent);
        }
    }

    public void GoToTutorial()
    {
        TransitionManager.Instance.SceneFadeIn(0.5f, () =>
            SceneManager.LoadScene("Tutorial"));
    }
}
