using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Seiryu Combo", menuName = "Enemy/Seiryu/nAttack2")]
public class Seiryu_nAttack2 : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        context.Anim.SetTrigger("Attack 2");

        await Task.Delay(1000);
        state.OnAttacked();
    }
}
