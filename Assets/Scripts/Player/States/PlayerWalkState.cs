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
        Vector3 movementDirection = Isometric.ToIso(_context.AxisInput).normalized;
        float speedModifier = _context.MoveSpeed * (_context.playerManager.soulBerserk ? 1.2f : 1f);
        Vector3 movement = movementDirection * speedModifier;

        _context.rigidBody.velocity = new Vector3(movement.x, _context.rigidBody.velocity.y, movement.z);
    }

    private void CheckChangeState()
    {
        if (_context.PressedMove && !_context.PressedSprint)
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
        else if (_context.PressedSoul && _context.SoulReady())
        {
            ChangeState(_context.State.Soul());
        }
    }

    public override void Exit()
    {
        _context.MoveSpeed = 0;
        _context.Anim.SetFloat("Speed", 0);
        _context.rigidBody.velocity = new Vector3(0, _context.rigidBody.velocity.y, 0);
    }
}
