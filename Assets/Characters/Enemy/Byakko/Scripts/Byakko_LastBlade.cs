using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Byakko_LashBlade", menuName = "Enemy/Byakko/Last Blade")]
public class Byakko_LastBlade : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        context.Anim.applyRootMotion = true;
        context.Anim.SetTrigger("LastBlade");

        await Task.Delay(2200);
        state.OnAttacked();
    }
}
