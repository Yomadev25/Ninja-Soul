using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenbuHut : MonoBehaviour
{
    public enum Type
    {
        Portal,
        Manager
    }

    public int id;
    public Type type;
    public string scene;
    public Vector3 destination;

    [Header("Hut Properties")]
    [SerializeField]
    private Event _event;
    [SerializeField]
    private GameObject _exitPortal;

    private int enemyCount;

    private void Awake()
    {
        MessagingCenter.Subscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent, (sender, @event) =>
        {
            if (@event != _event) return;
            GenbuManager.Instance.SetHut(id, true);
            PlayerData.Instance.SetSpawnPoint(destination);
            _exitPortal.SetActive(true);
        });

        MessagingCenter.Subscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied, (sender) =>
        {
            EventManager.Instance.RemoveEvent(_event);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent);
        MessagingCenter.Unsubscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied);
    }

    private void Start()
    {
        switch (type)
        {
            case Type.Portal:
                bool isClear = GenbuManager.Instance.huts.First(x => x.id == id).isClear;
                if (isClear)
                {
                    gameObject.SetActive(false);
                }
                break;
            case Type.Manager:
                TransitionManager.Instance.SceneFadeOut();
                EventManager.Instance.ActivateEvent(_event);
                break;
            default:
                break;
        }
    }

    public void BackToGenbu()
    {       
        TransitionManager.Instance.SceneFadeIn(0.5f, () =>
                SceneManager.LoadScene("Genbu_1"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (type != Type.Portal) return;
        if (other.CompareTag("Player"))
        {
            PlayerData.Instance.SetSpawnPoint(default);
            TransitionManager.Instance.SceneFadeIn(0.5f, () =>
                SceneManager.LoadScene(scene));
        }
    }
}
