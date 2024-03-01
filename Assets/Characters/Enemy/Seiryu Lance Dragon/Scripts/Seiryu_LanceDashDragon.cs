using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Seiryu Combo", menuName = "Enemy/Seiryu/Lance Dash Dragon")]
public class Seiryu_LanceDashDragon : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        context.Anim.SetTrigger("Dash");

        LeanTween.move(context.gameObject, context.transform.position + (context.transform.forward * 3f), 0.5f).setOnComplete(async () =>
        {
            await Task.Delay(500);
            Transform target = context.GetCombatTarget();

            if (target != null)
            {
                context.Anim.SetTrigger("Dash");
                context.transform.LookAt(new Vector3(target.position.x, context.transform.position.y, target.position.z));
                LeanTween.move(context.gameObject, context.transform.position + (context.transform.forward * 3f), 0.5f).setOnComplete(async () =>
                {
                    await Task.Delay(1000);
                    state.OnAttacked();
                });
            }
            else
            {
                state.OnAttacked();
            }
        });
    }
}
