using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombo : ScriptableObject
{
    public string name;
    [TextArea(5, 10)]
    public string description;

    [Header("Properties")]
    public float cooldown;
    public int damage;
    public float combatRange;

    public virtual async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        
    }
}
