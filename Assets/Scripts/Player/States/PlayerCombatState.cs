using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        MoveForward();
    }

    private async void MoveForward()
    {
        Vector3 dir = _context.transform.forward.normalized;
        float duration = 0.1f;
        while (duration > 0f)
        {
            _context.rigidBody.MovePosition(_context.transform.position + (dir) * Time.deltaTime);
            duration -= Time.deltaTime;

            await Task.Yield();
        }
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
            if (Time.time - lastClicked <= 0.2f && comboCount < comboGroup.combos.Length - 1)
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
