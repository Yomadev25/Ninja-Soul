using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByakkoYard : MonoBehaviour
{
    [SerializeField]
    private Event _event;
    [SerializeField]
    private GameObject _door;
    [SerializeField]
    private GameObject _portal;

    [SerializeField]
    private ByakkoManager _manager;

    private void Awake()
    {
        MessagingCenter.Subscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent, (sender, @event) =>
        {
            if (@event != _event) return;
            _door.SetActive(false);
            _portal.SetActive(true);
        });

        MessagingCenter.Subscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied, (sender) =>
        {
            EventManager.Instance.ClearAllEvents();
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent);
        MessagingCenter.Unsubscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied);
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        if (_manager == null)
        {
            EventManager.Instance.ActivateEvent(_event);
        }
        else
        {
            yield return new WaitForSeconds(11f);
            EventManager.Instance.ActivateEvent(_event);
        }
    }
}
