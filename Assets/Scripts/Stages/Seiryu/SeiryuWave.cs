using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeiryuWave : MonoBehaviour
{
    [SerializeField]
    private Event[] _events;
    [SerializeField]
    private GameObject[] _enemies;
    [SerializeField]
    private GameObject _door;

    private int wave;

    private void Awake()
    {
        MessagingCenter.Subscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent, (sender, @event)=>
        {
            if (_events.Contains(@event))
            {
                Invoke(nameof(NextWave), 2f);
            }
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent);
    }

    void Start()
    {
        InitWave();
        _door.SetActive(true);
    }

    void InitWave()
    {
        EventManager.Instance.ActivateEvent(_events[wave]);
        _enemies[wave].SetActive(true);
    }

    void NextWave()
    {
        wave++;

        if (wave < 4)
        {
            InitWave();
        }
        else
        {
            _door.SetActive(false);
        }        
    }
}
