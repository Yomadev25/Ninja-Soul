using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Event")]
public class Event : ScriptableObject
{
    public enum EventType
    {
        Eliminate,
        Collect,
        Custom,
    }

    public string title;
    public EventType type;
    [TextArea(5, 10)]
    public string description;

    [Header("Eliminate Condition")]
    public EliminateEvent[] eliminateEvents;
}

[System.Serializable]
public class EliminateEvent
{
    [HideInInspector]
    public Event Event;
    public Enemy enemy;
    public int targetCount;
    public int count;

    public EliminateEvent(Event @event, Enemy enemy, int targetCount, int count)
    {
        Event = @event;
        this.enemy = enemy;
        this.targetCount = targetCount;
        this.count = count;
    }
}
