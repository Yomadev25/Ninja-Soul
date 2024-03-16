using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Byakko_TigerSlash", menuName = "Enemy/Byakko/Tiger Slash")]
public class Byakko_TigerSlash : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        context.Anim.applyRootMotion = true;
        context.Anim.SetTrigger("TigerSlash");

        await Task.Delay(1200);
        state.OnAttacked();
    }
}
