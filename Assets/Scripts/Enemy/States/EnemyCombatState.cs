using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatState : EnemyBaseState
{
    public const string MessageOnExitCombatState = "On Exit Combat State";

    Enemy enemy;
    public EnemyCombatState(EnemyStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
        MessagingCenter.Subscribe<EnemyStateMachine>(this, EnemyStateMachine.MessageOnKnockdown, (sender) =>
        {
            if (sender != _context) return;
            ChangeState(_context.State.Knock());
        });

        MessagingCenter.Subscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyDead, (sender) =>
        {
            if (sender.stateMachine == _context)
            {
                ChangeState(_context.State.Idle());
            }
        });

        enemy = _context.Enemy;
        _context.Anim.SetFloat("Speed", 0);
        enemy.combos[_context.ComboCount].Execute(_context, this);
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {

    }

    public void OnAttacked()
    {
        if (_context.CurrentState != this) return;
        ChangeState(_context.State.Chase());
    }

    public override void Exit()
    {
        _context.Anim.applyRootMotion = false;
        _context.ResetCombatCooldown(enemy.combos[_context.ComboCount].cooldown);
        enemy = null;

        MessagingCenter.Unsubscribe<EnemyStateMachine>(this, EnemyStateMachine.MessageOnKnockdown);
        MessagingCenter.Unsubscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyDead);
        MessagingCenter.Send(this, MessageOnExitCombatState, _context);
    }
}
