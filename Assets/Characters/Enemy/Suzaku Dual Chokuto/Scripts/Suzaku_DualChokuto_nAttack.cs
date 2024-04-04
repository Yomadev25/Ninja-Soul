using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Dual Chokuto", menuName = "Enemy/Suzaku Dual Chokuto/nAttack")]
public class Suzaku_DualChokuto_nAttack : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        context.Anim.SetTrigger("Attack");

        if (target != null)
        {
            LeanTween.move(context.gameObject, context.transform.position + (target.position - context.transform.position) / 2f, 0.5f).setOnComplete(() =>
            {
                LeanTween.move(context.gameObject, context.transform.position + (-context.transform.forward / 1), 0.3f).setOnComplete(async () =>
                {
                    await Task.Delay(1000);
                    state.OnAttacked();
                });
            });
        }
        else
        {
            state.OnAttacked();
        }
    }
}
