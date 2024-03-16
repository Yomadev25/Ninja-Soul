using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Byakko_TripleSlash", menuName = "Enemy/Byakko/Triple Slash")]
public class Byakko_TripleSlash : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        context.Anim.applyRootMotion = true;
        context.Anim.SetTrigger("TripleSlash");

        await Task.Delay(2200);
        state.OnAttacked();
    }
}
