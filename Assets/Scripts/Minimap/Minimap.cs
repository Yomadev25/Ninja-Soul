using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public const string MessageInitMapCamera = "Initialize Map Camera";

    [Header("Whole Map")]
    [SerializeField]
    private Camera _mapCamera;
    [SerializeField]
    private float _mapOrthographicSize;

    private Transform _target;

    private void Start()
    {
        _mapCamera.orthographicSize = _mapOrthographicSize;

        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SetPlayerTarget(playerTransform);

        if (_mapCamera)
        {
            MessagingCenter.Send(this, MessageInitMapCamera, _mapCamera);
        }
    }

    private void SetPlayerTarget(Transform player)
    {
        _target = player;
    }
}
