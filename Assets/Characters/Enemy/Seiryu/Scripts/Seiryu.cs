using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seiryu : MonoBehaviour
{
    public const string MessageInitBossPhase = "Init Boss Phase";
    public const string MessageClearSeiryuStage = "Clear Seiryu Stage";

    [SerializeField]
    private Event _event;

    [SerializeField]
    private ParticleSystem _dashFx;
    [SerializeField]
    private GameObject _slashPrefab;
    [SerializeField]
    private GameObject _stormPrefab;

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
                MessagingCenter.Send(this, MessageClearSeiryuStage);
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
    }

    public void DashAttack()
    {
        StartCoroutine(DashCoroutine());
    }

    IEnumerator DashCoroutine()
    {
        _dashFx.Play();
        yield return new WaitForSeconds(0.5f);
        _dashFx.Stop();
    }

    public void LeftSlash()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y -= 90f;
        Instantiate(_slashPrefab, transform.position + transform.forward + Vector3.up, Quaternion.Euler(rotation));
    }

    public void RightSlash()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y -= 90f;
        Instantiate(_slashPrefab, transform.position + transform.forward + Vector3.up, Quaternion.Euler(rotation));
    }

    public void DragonStorm()
    {
        Instantiate(_stormPrefab, transform.position, Quaternion.Euler(-90f, 0f, 0f));

        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerManager player = collider.GetComponent<PlayerManager>();
                player.TakeDamage(1);
                break;
            }
        }
    }
}
