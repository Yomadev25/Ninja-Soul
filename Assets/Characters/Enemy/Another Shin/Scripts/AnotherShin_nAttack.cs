using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Another Shin Combo", menuName = "Enemy/Another Shin/nAttack")]
public class AnotherShin_nAttack : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        context.Anim.SetTrigger("Chokuto");

        if (target != null)
        {
            LeanTween.move(context.gameObject, context.transform.position + (target.position - context.transform.position) / 2f, 0.5f).setOnComplete(async () =>
            {
                await Task.Delay(2300);
                state.OnAttacked();
            });
        }
        else
        {
            state.OnAttacked();
        }
    }
}
