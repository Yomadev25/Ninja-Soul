using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Genbu Punch", menuName = "Enemy/Genbu/Punch")]
public class Genbu_Punch : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        int side = Random.Range(0, 2);
        context.Anim.SetTrigger(side == 0 ? "PunchL" : "PunchR");
        AudioManager.Instance.PlaySFX("Punch");

        LeanTween.move(context.gameObject, context.transform.position + (target.position - context.transform.position) / 2f, 0.5f).setOnComplete(async () =>
        {
            await Task.Delay(1000);
            state.OnAttacked();
        });
    }
}
