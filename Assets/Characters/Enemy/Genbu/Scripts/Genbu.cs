using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genbu : MonoBehaviour
{
    [SerializeField]
    private Event _event;

    [Header("Punch")]
    [SerializeField]
    private Transform _punchHand;

    [Header("Throw Skill")]
    [SerializeField]
    private GameObject _handRock;
    [SerializeField]
    private GameObject _rockPrefab;

    [Header("Stomp Skill")]
    [SerializeField]
    private GameObject _stompPrefab;

    private int _phase = 1;

    private void Awake()
    {
        MessagingCenter.Subscribe<Genbu_Throw>(this, Genbu_Throw.MessagePrepareRock, (sender) =>
        {
            _handRock.SetActive(true);
        });

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
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<Genbu_Throw>(this, Genbu_Throw.MessagePrepareRock);
    }

    private void Start()
    {
        EventManager.Instance.ActivatedEvent(_event);
    }

    public void Punch()
    {
        Collider[] colliders = Physics.OverlapSphere(_punchHand.position, 1f);
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

    public void Stomp()
    {
        Instantiate(_stompPrefab, transform.position, Quaternion.Euler(90, 0, 0));
        StartCoroutine(StompDamage());
    }

    IEnumerator StompDamage()
    {
        for (int i = 0; i < 3; i++)
        {
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
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Throw()
    {
        _handRock.SetActive(false);
        int count = 1;

        switch (_phase)
        {
            case 1:
                count = 3;
                break;
            case 2:
                count = 4;
                break;
            case 3:
                count = 6;
                break;
            default:
                count = 1;
                break;
        }

        for (int i = 0; i < count; i++)
        {
            GameObject Go = Instantiate(_rockPrefab, _handRock.transform.position, Quaternion.identity);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 targetPos = player.transform.position;
            targetPos.x = Random.Range(targetPos.x - 2, targetPos.x + 2);
            targetPos.z = Random.Range(targetPos.z - 2, targetPos.z + 2);
            Go.GetComponent<Projectile>().InitProjectile(transform.position, targetPos);
        }
    }
}
