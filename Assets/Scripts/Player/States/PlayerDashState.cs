using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
        _context.CanRotate = false;
        DashAsync();     
    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {

    }

    private async Task DashAsync()
    {
        Vector3 dashDirection = _context.transform.forward.normalized;
        float duration = 0.2f;

        while (duration > 0f)
        {
            _context.rigidBody.MovePosition(_context.transform.position + (dashDirection) * _context.DashSpeed * Time.deltaTime);
            duration -= Time.deltaTime;

            await Task.Yield(); // Yield control back to the main thread briefly.
        }

        CheckChangeState();
    }

    private void CheckChangeState()
    {
        if (_context.PressedMove)
        {
            ChangeState(_context.State.Walk());
        }
        else
        {
            ChangeState(_context.State.Idle());
        }
    }

    public override void Exit()
    {
        _context.CanRotate = true;
    }
}
