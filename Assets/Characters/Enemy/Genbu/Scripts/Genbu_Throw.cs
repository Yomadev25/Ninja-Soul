using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Genbu Combo", menuName = "Enemy/Genbu/Throw")]
public class Genbu_Throw : EnemyCombo
{
    public const string MessagePrepareRock = "Prepare Rock";

    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        MessagingCenter.Send(this, MessagePrepareRock);

        Transform target = context.GetCombatTarget();
        context.Anim.SetTrigger("Throw");

        await Task.Delay(4000);
        state.OnAttacked();
    }
}
