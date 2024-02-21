using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Genbu Combo", menuName = "Enemy/Genbu/Stomp")]
public class Genbu_Stomp : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        context.Anim.applyRootMotion = true;
        Transform target = context.GetCombatTarget();
        context.Anim.SetTrigger("Stomp");

        await Task.Delay(3500);
        state.OnAttacked();
    }
}
