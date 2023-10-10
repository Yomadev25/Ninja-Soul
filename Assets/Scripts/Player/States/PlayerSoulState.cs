using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoulState : PlayerBaseState
{
    float timePassed;
    float clipSpeed;

    public PlayerSoulState(PlayerStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
        _context.CanRotate = false;
        _context.Anim.SetTrigger("Soul");

        LeanTween.value(0, 1, 2f).setEaseOutSine().setOnUpdate((value) =>
        {
            _context.soulVolume.weight = value;
        });
    }

    public override void Update()
    {
        timePassed += Time.deltaTime;
        clipSpeed = _context.Anim.GetCurrentAnimatorStateInfo(1).speed;

        if (timePassed >= 2.1f / clipSpeed)
        {
            _context.playerManager.ActivateSoulBerserk();
            ChangeState(_context.State.Idle());
        }
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Exit()
    {
        _context.CanRotate = true;
    }
}
