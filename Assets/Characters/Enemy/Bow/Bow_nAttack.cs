using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Bow Combo", menuName = "Enemy/Bow/nAttack")]
public class Bow_nAttack : EnemyCombo
{
    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        Transform target = context.GetCombatTarget();
        
        if (target != null)
        {
            context.Anim.SetTrigger("Attack");
            float duration = 4.4f;
            while (duration > 0)
            {
                Vector3 targetPos = target.position;
                targetPos.y = context.transform.position.y;

                context.transform.LookAt(targetPos);
                duration -= Time.deltaTime;
                await Task.Yield();
            }
            state.OnAttacked();
        }
        else
        {
            state.OnAttacked();
        }
    }
}
