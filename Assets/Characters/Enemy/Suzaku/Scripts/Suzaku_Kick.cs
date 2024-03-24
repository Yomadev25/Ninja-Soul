using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Suzaku_Kick", menuName = "Enemy/Suzaku/Kick")]
public class Suzaku_Kick : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        context.Anim.SetTrigger("Kick");

        if (target != null)
        {
            LeanTween.move(context.gameObject, context.transform.position + (target.position - context.transform.position) / 2f, 0.5f).setOnComplete(async () =>
            {
                await Task.Delay(1000);
                state.OnAttacked();
            });
        }
        else
        {
            state.OnAttacked();
        }
    }
}
