using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
        
    }

    public override void Update()
    {
        _context.MoveSpeed = Mathf.Lerp(_context.MoveSpeed, _context.Speed, 20f * Time.deltaTime);
        _context.Anim.SetFloat("Speed", _context.MoveSpeed);

        CheckChangeState();
    }

    public override void FixedUpdate()
    {
        _context.rigidBody.MovePosition(_context.transform.position + (Isometric.ToIso(_context.AxisInput) * _context.AxisInput.normalized.magnitude) * _context.MoveSpeed * Time.deltaTime);
    }

    private void CheckChangeState()
    {
        if (_context.PressedMove && _context.PressedSprint)
        {
            ChangeState(_context.State.Run());
        }
        else if (!_context.PressedMove)
        {
            ChangeState(_context.State.Idle());
        }
        else if (_context.PressedDash)
        {
            ChangeState(_context.State.Dash());
        }
        else if (_context.PressedCombat)
        {
            ChangeState(_context.State.Combat());
        }
    }

    public override void Exit()
    {

    }
}
