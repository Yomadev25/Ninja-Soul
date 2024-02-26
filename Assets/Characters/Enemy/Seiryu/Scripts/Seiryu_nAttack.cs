using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Seiryu Combo", menuName = "Enemy/Seiryu/nAttack")]
public class Seiryu_nAttack : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        context.Anim.SetTrigger("Attack 1");

        LeanTween.move(context.gameObject, context.transform.position + (context.transform.forward * 5f), 0.2f).setOnComplete(async () =>
        {
            await Task.Delay(1000);
            state.OnAttacked();
        });
    }
}
