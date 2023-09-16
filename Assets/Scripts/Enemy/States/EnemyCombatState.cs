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
    }

    public override void Update()
    {
        CheckChangeState();
    }

    public override void FixedUpdate()
    {

    }

    private void CheckChangeState()
    {

    }

    public override void Exit()
    {
        enemy = null;
    }
}
