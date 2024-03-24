using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Suzaku_nAttack", menuName = "Enemy/Suzaku/nAttack")]
public class Suzaku_nAttack1 : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        int combo = Random.Range(0, 2);
        context.Anim.SetTrigger(combo == 0 ? "Attack1" : "Attack2");

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
