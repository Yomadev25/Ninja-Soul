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
