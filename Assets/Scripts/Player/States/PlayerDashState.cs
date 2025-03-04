using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerDashState : PlayerBaseState
{
    public const string MessageOnDashStart = "Dash Start";
    public const string MessageOnDashEnd = "Dash End";
    bool isDash;

    public PlayerDashState(PlayerStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
        _context.CanRotate = false;
        _context.Anim.applyRootMotion = true;
        MessagingCenter.Send(this, MessageOnDashStart);

        DashAsync();     
    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {
        if (!isDash) return;

        Vector3 dashDirection = _context.transform.forward.normalized;
        Vector3 force = dashDirection * _context.DashSpeed;
        _context.rigidBody.velocity = new Vector3(force.x, _context.rigidBody.velocity.y, force.z);
    }

    private async Task DashAsync()
    {
        float duration = 0.8f;       
        _context.Anim.SetTrigger("Dash");
        _context.Anim.SetBool("isDash", true);

        while (duration > 0f)
        {
            isDash = true;
            duration -= Time.deltaTime;

            await Task.Yield();
        }

        _context.rigidBody.velocity = Vector3.zero;

        _context.Anim.SetBool("isDash", false);
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
        _context.Anim.applyRootMotion = false;
        MessagingCenter.Send(this, MessageOnDashEnd);
    }
}
