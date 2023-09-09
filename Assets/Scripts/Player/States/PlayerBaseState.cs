using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateMachine _context;

    public PlayerBaseState(PlayerStateMachine ctx)
    {
        _context = ctx;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();

    protected void ChangeState(PlayerBaseState newState)
    {
        _context.CurrentState.Exit();
     
        newState.Enter();
        _context.CurrentState = newState;
    }
}
