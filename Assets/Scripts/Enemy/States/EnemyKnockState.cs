using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockState : EnemyBaseState
{
    Enemy enemy;

    public EnemyKnockState(EnemyStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
        enemy = _context.Enemy;
        _context.Anim.SetTrigger("Knock");
        _context.Anim.SetBool("isKnock", true);

        MessagingCenter.Subscribe<EnemyStateMachine>(this, EnemyStateMachine.MessageOnStandUp, (sender) =>
        {
            if (sender != _context) return;
            ChangeState(_context.State.Idle());
        });

        TimeStop.Instance.StopTime(0.05f, 10, 0.1f);
        AudioManager.Instance.PlaySFX("Kill");
        GameObject effect = EffectManager.Instance.Spawn("Kill Impact", _context.transform.position + Vector3.up, Quaternion.identity);
        _context.DestroyGameObject(effect, 1f);
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {
        _context.Anim.SetBool("isKnock", false);
        MessagingCenter.Unsubscribe<EnemyStateMachine>(this, EnemyStateMachine.MessageOnStandUp);
    }
}
