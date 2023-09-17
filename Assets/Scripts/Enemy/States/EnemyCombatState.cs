using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatState : EnemyBaseState
{
    Enemy enemy;
    public EnemyCombatState(EnemyStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
        enemy = _context.Enemy;
        _context.Anim.SetFloat("Speed", 0);

        //if Want To Random Combo
        int randomCombo = Random.Range(0, enemy.combos.Length);
        _context.ComboCount = randomCombo;
        //else Want To Set Combo

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
        enemy = null;
        _context.ResetCombatCooldown();
    }
}
