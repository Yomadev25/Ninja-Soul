using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrepareState : EnemyBaseState
{
    Enemy enemy;
    Vector3 direction;
    float moveSpeed = 1f;

    Transform target;
    GameObject alertIcon;

    public EnemyPrepareState(EnemyStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
        MessagingCenter.Subscribe<EnemyStateMachine>(this, EnemyStateMachine.MessageOnKnockdown, (sender) =>
        {
            if (sender != _context) return;
            ChangeState(_context.State.Knock());
        });

        enemy = _context.Enemy;

        int randomDir = Random.Range(0, 2);
        direction = randomDir == 1 ? Vector3.right : Vector3.left;

        _context.Anim.SetFloat("Speed", moveSpeed);
    }

    public override void Update()
    {
        target = _context.GetCombatTarget();
        if (target != null)
        {
            Vector3 dir = (target.position - _context.transform.position).normalized;
            Vector3 pDir = Quaternion.AngleAxis(90, Vector3.up) * dir;
            Vector3 movedir = Vector3.zero;
            Vector3 finalDirection = (pDir * direction.normalized.x);

            movedir += finalDirection * moveSpeed * Time.deltaTime;
            _context.NavMesh.Move(movedir);
            _context.transform.LookAt(new Vector3(target.position.x, _context.transform.position.y, target.position.z));
        }

        if (alertIcon == null)
        {
            if (_context.CurrentCooldown < 1)
            {
                alertIcon = EffectManager.Instance.Spawn("Attack Alert", _context.transform.position + (Vector3.up * 2), Quaternion.identity);
                alertIcon.transform.parent = _context.transform;
                alertIcon.transform.localScale = Vector3.zero;
                alertIcon.LeanScale(Vector3.one, 0.5f).setEaseInBack();
            }
        }

        CheckChangeState();
    }

    public override void FixedUpdate()
    {

    }

    private void CheckChangeState()
    {
        if (_context.IsReadyToCombat)
        {
            ChangeState(_context.State.Combat());
        }
        
        if (target == null)
        {
            ChangeState(_context.State.Chase());
        }
    }

    public override void Exit()
    {
        MessagingCenter.Unsubscribe<EnemyStateMachine>(this, EnemyStateMachine.MessageOnKnockdown);
        _context.DestroyGameObject(alertIcon);
        enemy = null;
    }
}
