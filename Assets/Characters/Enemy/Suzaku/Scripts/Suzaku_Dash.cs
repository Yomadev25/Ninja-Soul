using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Suzaku_Dash", menuName = "Enemy/Suzaku/Dash")]
public class Suzaku_Dash : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        context.Anim.SetTrigger("Dash");

        LeanTween.move(context.gameObject, context.transform.position + (context.transform.forward * 5f), 0.5f).setOnComplete(async () =>
        {
            await Task.Delay(800);
            state.OnAttacked();
        });
    }
}
