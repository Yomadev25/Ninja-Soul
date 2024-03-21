using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Byakko : MonoBehaviour
{
    public const string MessageClearByakkoStage = "Clear Byakko Stage";

    [SerializeField]
    private Event _event;

    [SerializeField]
    private GameObject _slashFx;
    [SerializeField]
    private GameObject _lastBladePrefab;

    private int _phase = 1;

    private void Awake()
    {
        MessagingCenter.Subscribe<EnemyManager>(this, EnemyManager.MessageOnUpdateHp, (sender) =>
        {
            if (sender.gameObject != gameObject) return;

            if ((sender.hp / sender.maxHp) <= 0.5f)
            {
                if (_phase == 2) return;
                _phase = 2;
            }
            else if ((sender.hp / sender.maxHp) <= 0.2f)
            {
                if (_phase == 3) return;
                _phase = 3;
            }
        });

        MessagingCenter.Subscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent, (sender, @event) =>
        {
            if (@event == _event)
            {
                MessagingCenter.Send(this, MessageClearByakkoStage);
            }
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<EnemyManager>(this, EnemyManager.MessageOnUpdateHp);
        MessagingCenter.Unsubscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent);
    }

    private void Start()
    {
        EventManager.Instance.ActivateEvent(_event);
    }

    public void Slash()
    {
        GameObject slashFx = Instantiate(_slashFx, new Vector3(transform.localPosition.x, transform.position.y + 1.1f, transform.localPosition.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        Destroy(slashFx, 0.5f);
    }

    public void TigerSlash()
    {
        GameObject slashFx = Instantiate(_slashFx, new Vector3(transform.localPosition.x, transform.position.y + 1.1f, transform.localPosition.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        Destroy(slashFx, 0.5f);
    }

    public void TripleSlash(int combo)
    {
        GameObject slashFx = Instantiate(_slashFx, new Vector3(transform.localPosition.x, transform.position.y + 1.1f, transform.localPosition.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        switch (combo)
        {
            case 1:
                eulerAngle = new Vector3(125f, transform.eulerAngles.y, 0f);
                break;
            case 2:
                eulerAngle = new Vector3(65f, transform.eulerAngles.y, 0f);
                break;
            case 3:
                eulerAngle = new Vector3(125f, transform.eulerAngles.y, 0f);
                break;
            default:
                break;
        }

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        Destroy(slashFx, 0.5f);
    }

    public void LastBlade()
    {
        Instantiate(_lastBladePrefab,
            new Vector3(transform.position.x, transform.position.y + 1, transform.position.z) + transform.forward,
            Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y - 90f, 0f)));
    }
}
