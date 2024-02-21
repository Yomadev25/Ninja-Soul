using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genbu : MonoBehaviour
{
    [SerializeField]
    private Event _event;

    [Header("Throw Skill")]
    [SerializeField]
    private GameObject _handRock;
    [SerializeField]
    private GameObject _rockPrefab;

    [Header("Stomp Skill")]
    [SerializeField]
    private GameObject _stompPrefab;

    private void Awake()
    {
        MessagingCenter.Subscribe<Genbu_Throw>(this, Genbu_Throw.MessagePrepareRock, (sender) =>
        {
            _handRock.SetActive(true);
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
        Debug.Log("Punch");
    }

    public void Stomp()
    {
        Instantiate(_stompPrefab, transform.position, Quaternion.Euler(90, 0, 0));
        CameraShake.instance.InstantShake(2f);
    }

    public void Throw()
    {
        _handRock.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            GameObject Go = Instantiate(_rockPrefab, _handRock.transform.position, Quaternion.identity);
            Go.GetComponent<Projectile>().InitProjectile(transform.position, transform.forward * Random.Range(5f, 15f));
        }
    }
}
