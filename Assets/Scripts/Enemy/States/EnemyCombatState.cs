using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class EnemyCombatState : EnemyBaseState
{
    Enemy enemy;
    public EnemyCombatState(EnemyStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
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
        ChangeState(_context.State.Chase());
    }

    public override void Exit()
    {
        _context.Anim.applyRootMotion = false;
        _context.ResetCombatCooldown(enemy.combos[_context.ComboCount].cooldown);
        enemy = null;     
    }
}
