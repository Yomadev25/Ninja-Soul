using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Suzaku : MonoBehaviour
{
    public const string MessageInitBossPhase = "Init Boss Phase";
    public const string MessageClearSuzakuStage = "Clear Suzaku Stage";

    [SerializeField]
    private Event _event;

    [SerializeField]
    private GameObject _slashFx;
    [SerializeField]
    private VisualEffect _dashFx;
    [SerializeField]
    private GameObject _kickFx;
    [SerializeField]
    private GameObject _fireBallPrefab;

    [Header("Enemies Wave")]
    [SerializeField]
    private GameObject[] _enemyWaves;

    private int _phase = 1;

    private void Awake()
    {
        MessagingCenter.Subscribe<EnemyManager>(this, EnemyManager.MessageOnUpdateHp, (sender) =>
        {
            if (sender.gameObject != gameObject) return;

            if ((sender.hp / sender.maxHp) <= 0.5f)
            {
                if (_phase < 2)
                {
                    if (TryGetComponent(out EnemyStateMachine state))
                    {
                        state.Knockdown();
                    }

                    _phase = 2;
                }
            }
            if ((sender.hp / sender.maxHp) <= 0.25f)
            {
                if (_phase < 3)
                {
                    if (TryGetComponent(out EnemyStateMachine state))
                    {
                        state.Knockdown();
                    }

                    _phase = 3;
                }
            }

            if (_enemyWaves.Length > _phase - 1)
            {
                if (_enemyWaves[_phase - 1] != null)
                    _enemyWaves[_phase - 1].SetActive(true);
            }
        });

        MessagingCenter.Subscribe<EnemyStateMachine>(this, EnemyStateMachine.MessageOnStandUp, (sender) =>
        {
            switch (_phase)
            {
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }

            MessagingCenter.Send(this, MessageInitBossPhase, _phase);
        });

        MessagingCenter.Subscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent, (sender, @event) =>
        {
            if (@event == _event)
            {
                MessagingCenter.Send(this, MessageClearSuzakuStage);
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
        if (_event != null)
         EventManager.Instance.ActivateEvent(_event);

        if (AudioManager.Instance.currentBgm != "Final Boss")
            AudioManager.Instance.PlayBGM("Suzaku");
    }

    public void Attack1(int combo)
    {
        GameObject slashFx = Instantiate(_slashFx, new Vector3(transform.localPosition.x, transform.position.y + 1.1f, transform.localPosition.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        switch (combo)
        {
            case 1:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, 180f);
                break;
            case 2:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, 0f);
                break;
            case 3:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, 0f);
                break;
            default:
                break;
        }

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        Destroy(slashFx, 0.5f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out PlayerManager player))
            {
                player.TakeDamage(1);
            }
        }
    }

    public void Attack2(int combo)
    {
        GameObject slashFx = Instantiate(_slashFx, new Vector3(transform.localPosition.x, transform.position.y + 1.1f, transform.localPosition.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        switch (combo)
        {
            case 1:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -10f);
                break;
            case 2:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -20f);
                break;
            case 3:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -205f);
                break;
            case 4:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -170f);
                break;
            default:
                break;
        }

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        Destroy(slashFx, 0.5f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out PlayerManager player))
            {
                player.TakeDamage(1);
            }
        }
    }

    public void Attack3(int combo)
    {
        GameObject slashFx = Instantiate(_slashFx, new Vector3(transform.localPosition.x, transform.position.y + 1.1f, transform.localPosition.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        switch (combo)
        {
            case 1:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -135f);
                break;
            case 2:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -17f);
                break;
            case 3:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -40f);

                GameObject slashFx2 = Instantiate(_slashFx, new Vector3(transform.localPosition.x, transform.position.y + 1.1f, transform.localPosition.z) + (transform.forward * 0.6f), Quaternion.identity);
                Vector3 eulerAngle2 = new Vector3(0f, transform.eulerAngles.y, -120f);
                slashFx2.transform.localEulerAngles = eulerAngle2;
                slashFx2.GetComponentInChildren<VisualEffect>().Play();
                Destroy(slashFx2, 0.5f);
                break;
            default:
                break;
        }

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        Destroy(slashFx, 0.5f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out PlayerManager player))
            {
                player.TakeDamage(1);
            }
        }
    }

    public void Dash()
    {
        _dashFx.Play();
        AudioManager.Instance.PlaySFX("Fire Dash");
        if (_phase > 1)
        {
            SpawnFireball();

            for (int i = 0; i < _phase - 1; i++)
            {
                Invoke(nameof(SpawnFireball), 1f);
            }
        }
    }

    private void SpawnFireball()
    {
        Instantiate(_fireBallPrefab, transform.position + Vector3.up, Quaternion.identity)
            .GetComponent<Fireball>().owner = transform;
    }

    public void Kick()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out PlayerManager player))
            {
                player.TakeDamage(1);
            }
        }

        AudioManager.Instance.PlaySFX("Punch");
    }
}
