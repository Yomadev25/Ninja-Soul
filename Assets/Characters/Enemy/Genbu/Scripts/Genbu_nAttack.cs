using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Genbu Combo", menuName = "Enemy/Genbu/nAttack")]
public class Genbu_nAttack : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        context.Anim.SetTrigger("nAttack");
        
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
