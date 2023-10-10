using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentFactory : MonoBehaviour
{
    [SerializeField]
    private Weapon _weapon;
    [SerializeField]
    private PlayerStateMachine _playerStateMachine;

    void Start()
    {
        if (_weapon == null)
        {
            Debug.LogError("This character doesn't equip weapon");
        }
        if (_playerStateMachine == null)
        {
            Debug.LogErrorFormat("{0} doesn't exist", nameof(PlayerStateMachine));
        }
    }

    public void StartDealWeaponDamage()
    {
        int currentCombo = _playerStateMachine.ComboCount;
        float damageAdjust = _playerStateMachine.ComboFactory.Combos[currentCombo].damage * (_playerStateMachine.playerManager.soulBerserk? 2f : 1f);

        _weapon.StartDealDamage(damageAdjust, _playerStateMachine.ComboFactory.Combos[currentCombo].eulerAngle);
    }

    public void EndDealWeaponDamage()
    {
        _weapon.EndDealDamage();
    }
}
