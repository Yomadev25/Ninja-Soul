using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofJumper : MonoBehaviour, IInteract
{
    [SerializeField]
    private CinemachineDollyCart _dollyCart;
    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void DisableInteract()
    {
        
    }

    public void EnableInteract()
    {
        
    }

    public void Interact()
    {
        _playerTransform.position = _dollyCart.transform.position;
        _playerTransform.rotation = transform.rotation;
        PlayerStateMachine playerStateMachine = _playerTransform.GetComponent<PlayerStateMachine>();
        playerStateMachine.Anim.SetTrigger("Jump");

        LeanTween.value(0, 1, 2f).setDelay(0.3f)
        .setOnUpdate(x =>
        {
            _dollyCart.m_Position = x;
            _playerTransform.position = _dollyCart.transform.position;
        }).setOnComplete(() =>
        {
            _dollyCart.m_Position = 0;
            playerStateMachine.Anim.SetTrigger("Landing");
        });
    }
}
