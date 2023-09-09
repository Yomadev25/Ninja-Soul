using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatState : PlayerBaseState
{
    float timePassed;
    float clipLength;
    float clipSpeed;

    int comboCount;
    float lastClicked;

    public PlayerCombatState(PlayerStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
        comboCount = _context.ComboCount;

        _context.Anim.applyRootMotion = true;
        timePassed = 0f;

        var comboAnim = _context.ComboFactory.Combos[comboCount].animation;
        _context.Anim.runtimeAnimatorController = comboAnim;
        _context.Anim.Play("Attack", 1, 0);

        clipLength = comboAnim["nAttack1"].length;     
    }

    public override void Update()
    {
        if (_context.PressedCombat)
        {
            lastClicked = Time.time;
        }

        timePassed += Time.deltaTime;
        clipSpeed = _context.Anim.GetCurrentAnimatorStateInfo(1).speed;

        CheckChangeState();
    }

    public override void FixedUpdate()
    {
        
    }

    private void CheckChangeState()
    {
        if (timePassed >= clipLength / clipSpeed)
        {
            if (Time.time - lastClicked <= 0.2f && comboCount < _context.ComboFactory.Combos.Length - 1)
            {
                _context.ComboCount++;
                ChangeState(_context.State.Combat());

                return;
            }

            _context.ComboCount = 0;
            ChangeState(_context.State.Idle());         
        }
    }

    public override void Exit()
    {
        _context.Anim.applyRootMotion = false;
    }
}
