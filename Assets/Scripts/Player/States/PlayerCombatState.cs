using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCombatState : PlayerBaseState
{
    float timePassed;
    float clipLength;
    float clipSpeed;

    int comboCount;
    float lastClicked;

    ComboGroup comboGroup;

    public PlayerCombatState(PlayerStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
        _context.CanRotate = false;
        comboCount = _context.ComboCount;

        _context.Anim.applyRootMotion = true;
        timePassed = 0f;

        comboGroup = _context.ComboFactory.ComboGroups.First(x => x.name == _context.playerEquipment.weapons[0].WeaponName);
        var comboAnim = comboGroup.combos[comboCount].animation;
        _context.Anim.runtimeAnimatorController = comboAnim;
        _context.Anim.Play("Attack", 1, 0);

        clipLength = comboAnim["nAttack1"].length;
    }

    public override void Update()
    {
        if (_context.CombatInputBuffered)
        {
            lastClicked = Time.time;
            _context.CombatInputBuffered = false;
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
        float attackEndTime = clipLength / clipSpeed;
        bool bufferedInput = Time.time - lastClicked <= 0.2f && comboCount < comboGroup.combos.Length - 1;

        // Let a buffered press cancel into the next attack as soon as the
        // current swing's recovery window opens, instead of waiting for the
        // whole clip (startup + active + recovery) to finish playing.
        float cancelTime = attackEndTime * _context.ComboCancelWindow;
        if (bufferedInput && timePassed >= cancelTime)
        {
            _context.ComboCount++;
            ChangeState(_context.State.Combat());

            return;
        }

        if (timePassed >= attackEndTime)
        {
            if (bufferedInput)
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
        _context.CanRotate = true;
    }
}
