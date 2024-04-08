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
        MessagingCenter.Subscribe<EnemyStateMachine>(this, EnemyStateMachine.MessageOnKnockdown, (sender) =>
        {
            if (sender != _context) return;
            ChangeState(_context.State.Knock());
        });

        enemy = _context.Enemy;
        _context.NavMesh.isStopped = false;

        //if Want To Random Combo
        int randomCombo = Random.Range(0, enemy.combos.Length);
        _context.ComboCount = randomCombo;
        //else Want To Set Combo

        _context.SetCombatRadius(_context.Enemy.combos[_context.ComboCount].combatRange);
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
            ChangeState(_context.State.Prepare());
        }
    }

    public override void Exit()
    {
        MessagingCenter.Unsubscribe<EnemyStateMachine>(this, EnemyStateMachine.MessageOnKnockdown);

        _context.NavMesh.velocity = Vector3.zero;
        _context.NavMesh.Stop();

        enemy = null;
    }
}
