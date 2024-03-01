using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Seiryu Combo", menuName = "Enemy/Seiryu/Lance Dash")]
public class Seiryu_LanceDash : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        context.Anim.SetTrigger("Dash");

        LeanTween.move(context.gameObject, context.transform.position + (context.transform.forward * 3f), 0.5f).setOnComplete(async () =>
        {
            await Task.Delay(1000);
            state.OnAttacked();
        });
    }
}
