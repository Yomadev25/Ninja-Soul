using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
        
    }

    public override void Update()
    {
        _context.MoveSpeed = Mathf.Lerp(_context.MoveSpeed, 0, 20f * Time.deltaTime);
        _context.Anim.SetFloat("Speed", _context.MoveSpeed);

        CheckChangeState();
    }

    public override void FixedUpdate()
    {
        
    }

    private void CheckChangeState()
    {
        if (_context.PressedMove)
        {
            ChangeState(_context.State.Run());
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
        
    }
}
