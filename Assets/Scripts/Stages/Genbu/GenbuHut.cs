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
            GenbuManager.Instance.SetHut(id, true);
            _exitPortal.SetActive(true);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent);
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
                EventManager.Instance.ActivatedEvent(_event);
                break;
            default:
                break;
        }
    }

    public void BackToGenbu()
    {
        TransitionManager.Instance.SceneFadeIn(1, () =>
                SceneManager.LoadScene("Genbu_1"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (type != Type.Portal) return;
        if (other.CompareTag("Player"))
        {
            TransitionManager.Instance.SceneFadeIn(1, () =>
                SceneManager.LoadScene(scene));
        }
    }
}
