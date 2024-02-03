using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public const string MessageActivateEvent = "Activate Event";
    public const string MessageOnUpdateEvent = "On Update Event Progress";
    public const string MessageOnArchievedEvent = "On Archieved Event";

    private List<Event> _events = new List<Event>();
    private List<EliminateEvent> _eliminateEvents = new List<EliminateEvent>();

    public List<Event> Events => _events;
    public List<EliminateEvent> EliminateEvents => _eliminateEvents;

    protected override void Awake()
    {
        base.Awake();

        MessagingCenter.Subscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyDead, (sender) =>
        {
            CheckEliminateEvent(sender);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyDead);
    }

    public void ActivatedEvent(Event _event)
    {
        if (!_events.Contains(_event))
        {
            _events.Add(_event);
            foreach (EliminateEvent eliminateEvent in _event.eliminateEvents)
            {
                EliminateEvent instanceEvent = new EliminateEvent(_event, eliminateEvent.enemy, eliminateEvent.targetCount, eliminateEvent.count);
                _eliminateEvents.Add(instanceEvent);
            }

            MessagingCenter.Send(this, MessageActivateEvent, _event);
            MessagingCenter.Send(this, MessageOnUpdateEvent);
        }
    }

    private void CheckEliminateEvent(EnemyManager enemy)
    {
        foreach (var _event in _events)
        {
            var eliminateEvents = _eliminateEvents.Where(x => x.Event == _event).ToArray();
            int completedTask = 0;

            foreach (var eliminateEvent in eliminateEvents)
            {
                if (eliminateEvent.count >= eliminateEvent.targetCount)
                {
                    completedTask++;
                    continue;
                }

                if (eliminateEvent.enemy == enemy.Enemy)
                {
                    eliminateEvent.count++;
                    MessagingCenter.Send(this, MessageOnUpdateEvent);

                    if (eliminateEvent.count >= eliminateEvent.targetCount)
                    {
                        completedTask++;
                    }
                }
            }

            Debug.Log(completedTask);
            if (completedTask >= _event.eliminateEvents.Length)
                ArchieveEvent(_event);
        }
    }

    private void CheckCollectingEvent()
    {

    }

    private void CheckDestinationEvent()
    {

    }

    public void ArchieveEvent(Event _event)
    {
        if (_events.Contains(_event))
        {
            _events.Remove(_event);
        }

        MessagingCenter.Send(this, MessageOnArchievedEvent, _event);
        MessagingCenter.Send(this, MessageOnUpdateEvent);
    }
}
