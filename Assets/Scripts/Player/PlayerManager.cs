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

    #region MESSAGE FOR PUB/SUB
    public const string MessageOnPlayerTookDamage = "Player Took Damage";
    public const string MessageOnPlayerDied = "Player Died";
    #endregion

    public bool IsDie { get; set; }

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
        MessagingCenter.Send(this, MessageOnPlayerTookDamage);
    }

    private void Die()
    {
        if (IsDie) return;

        IsDie = true;
        _playerStateMachine.enabled = false;

        _anim.applyRootMotion = true;
        _anim.SetTrigger("Die");

        MessagingCenter.Send(this, MessageOnPlayerDied);
    }
}
