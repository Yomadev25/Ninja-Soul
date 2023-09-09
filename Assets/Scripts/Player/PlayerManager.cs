using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour, IDamageable
{
    [Header("Properties")]
    [SerializeField]
    private float _maxHp;
    [SerializeField]
    private float _hp;

    [Header("References")]
    [SerializeField]
    private PlayerStateMachine _playerStateMachine;
    [SerializeField]
    private Animator _anim;

    [Header("Events")]
    [SerializeField]
    private UnityEvent onTakeDamage;
    [SerializeField]
    private UnityEvent onHeal;

    bool isDie;

    private void Start()
    {
        _hp = _maxHp;
    }

    private void Update()
    {
        if (_hp <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        _hp -= damage;
        _anim.SetTrigger("Hit");

        onTakeDamage?.Invoke();
    }

    private void Die()
    {
        if (isDie) return;

        isDie = true;
        _playerStateMachine.enabled = false;

        _anim.applyRootMotion = true;
        _anim.SetTrigger("Die");
    }
}
