using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Seiryu Combo", menuName = "Enemy/Seiryu/nAttack4")]
public class Seiryu_nAttack4 : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        context.Anim.SetTrigger("Attack 4");

        LeanTween.move(context.gameObject, context.transform.position + (context.transform.forward * 3f), 1f).setOnComplete(async () =>
        {
            await Task.Delay(1000);
            state.OnAttacked();
        });
    }
}
