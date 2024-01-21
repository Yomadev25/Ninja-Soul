using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EventHudManager : MonoBehaviour
{
    [SerializeField]
    private Transform _eventRoot;

    [Header("Event Template")]
    [SerializeField]
    public GameObject _eventTemplate;
    [SerializeField]
    private TMP_Text _eventName;
    [SerializeField]
    private TMP_Text _eventObjective;

    private void Awake()
    {
        MessagingCenter.Subscribe<EventManager>(this, EventManager.MessageOnUpdateEvent, (sender) =>
        {
            UpdateEventList(sender);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<EventManager>(this, EventManager.MessageOnUpdateEvent);
    }

    private void UpdateEventList(EventManager eventManager)
    {
        foreach (Transform item in _eventRoot)
        {
            Destroy(item.gameObject);
        }

        foreach (Event @event in eventManager.Events)
        {
            _eventName.text = @event.title;
            _eventObjective.text = "";

            switch (@event.type)
            {
                case Event.EventType.Eliminate:
                    var eliminateEvents = eventManager.EliminateEvents.Where(x => x.Event == @event).ToArray();
                    foreach (EliminateEvent eliminateEvent in eliminateEvents)
                    {
                        _eventObjective.text += $"{eliminateEvent.count}/{eliminateEvent.targetCount} {eliminateEvent.enemy.name}";
                        _eventObjective.text += "\n";
                    }
                    break;
                case Event.EventType.Collect:
                    break;
                case Event.EventType.Destination:
                    break;
                default:
                    break;
            }

            GameObject item = Instantiate(_eventTemplate, _eventRoot);
            item.SetActive(true);
        }
    }
}
