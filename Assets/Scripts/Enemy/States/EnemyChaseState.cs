using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    Enemy enemy;
    Transform target;

    public EnemyChaseState(EnemyStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
        enemy = _context.Enemy;
        _context.NavMesh.isStopped = false;
    }

    public override void Update()
    {
        target = _context.GetChasedTarget();

        if (target != null)
        {
            _context.NavMesh.SetDestination(target.position);
        }
        _context.Anim.SetFloat("Speed", _context.NavMesh.velocity.magnitude);

        CheckChangeState();
    }

    public override void FixedUpdate()
    {

    }

    private void CheckChangeState()
    {
        if (target == null)
        {
            ChangeState(_context.State.Idle());
        }
        
        if (_context.GetCombatTarget() != null)
        {
            ChangeState(_context.State.Prepare()); //Don't forget to change into Prepare State
        }
    }

    public override void Exit()
    {
        _context.NavMesh.velocity = Vector3.zero;
        _context.NavMesh.Stop();

        enemy = null;
    }
}
