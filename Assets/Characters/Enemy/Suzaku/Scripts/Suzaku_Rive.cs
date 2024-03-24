using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Suzaku_Rive", menuName = "Enemy/Suzaku/Rive")]
public class Suzaku_Rive : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        context.Anim.applyRootMotion = true;
        context.Anim.SetTrigger("Attack3");

        await Task.Delay(1500);
        state.OnAttacked();
    }
}
