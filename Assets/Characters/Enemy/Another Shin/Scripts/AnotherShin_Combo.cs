using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Another Shin Combo", menuName = "Enemy/Another Shin/Random Combo")]
public class AnotherShin_Combo : EnemyCombo
{
    [Header("Another Shin Combo")]
    public EnemyCombo[] hikariCombos;
    public EnemyCombo[] genbuCombos;
    public EnemyCombo[] suzakuCombos;
    public EnemyCombo[] seiryuCombos;
    public EnemyCombo[] byakkoCombos;

    public override async void Execute(EnemyStateMachine context, EnemyCombatState state)
    {
        AnotherShin anotherShin = context.GetComponent<AnotherShin>();
        int clan = (int)anotherShin.currentClan;
        EnemyCombo[] combos = null;
        switch (clan)
        {
            case 0:
                combos = hikariCombos;
                break;
            case 1:
                combos = genbuCombos;
                break;
            case 2:
                combos = suzakuCombos;
                break;
            case 3:
                combos = seiryuCombos;
                break;
            case 4:
                combos = byakkoCombos;
                break;
        }

        EnemyCombo combo = combos[Random.Range(0, combos.Length)];
        combo.Execute(context, state);
    }
}
