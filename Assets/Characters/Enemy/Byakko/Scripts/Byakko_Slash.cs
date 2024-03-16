using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Byakko Slash", menuName = "Enemy/Byakko/Slash")]
public class Byakko_Slash : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        context.Anim.SetTrigger("Slash");

        await Task.Delay(2000);
        state.OnAttacked();
    }
}
