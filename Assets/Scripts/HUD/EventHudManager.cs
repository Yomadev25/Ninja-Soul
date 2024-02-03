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

    [Header("Event Pop-up")]
    [SerializeField]
    private CanvasGroup _eventPopup;
    [SerializeField]
    private TMP_Text _eventPopupName;
    [SerializeField]
    private TMP_Text _eventPopupObjective;

    private void Awake()
    {
        MessagingCenter.Subscribe<EventManager, Event>(this, EventManager.MessageActivateEvent, (sender, @event) =>
        {
            EventPopup(@event);
        });

        MessagingCenter.Subscribe<EventManager>(this, EventManager.MessageOnUpdateEvent, (sender) =>
        {
            UpdateEventList(sender);
        });

        MessagingCenter.Subscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent, (sender, @event) =>
        {
            EventPopup(@event, true);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<EventManager, Event>(this, EventManager.MessageActivateEvent);
        MessagingCenter.Unsubscribe<EventManager>(this, EventManager.MessageOnUpdateEvent);
        MessagingCenter.Unsubscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent);
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
                case Event.EventType.Custom:
                    _eventObjective.text = @event.description;
                    break;
                default:
                    break;
            }

            GameObject item = Instantiate(_eventTemplate, _eventRoot);
            item.SetActive(true);
        }
    }

    private void EventPopup(Event @event, bool isComplete = false)
    {
        string name = "";
        if (isComplete)
        {
            name = "COMPLETED";
        }
        else
        {
            switch (@event.type)
            {
                case Event.EventType.Eliminate:
                    name = "AMBUSHED";
                    break;
                case Event.EventType.Collect:
                    name = "MISSION";
                    break;
                case Event.EventType.Custom:
                    name = "MISSION";
                    break;
                default:
                    name = "MISSION";
                    break;
            }
        }

        _eventPopupName.text = name;
        _eventPopupObjective.text = @event.title;

        LeanTween.cancel(_eventPopup.gameObject);
        _eventPopup.alpha = 0;
        _eventPopup.LeanAlpha(1, 1).setOnComplete(() =>
        {
            _eventPopup.LeanAlpha(0, 1).setDelay(1.5f);
        });
    }
}
