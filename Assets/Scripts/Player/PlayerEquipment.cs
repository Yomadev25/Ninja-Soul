using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField]
    private Weapon _weapon;
    [SerializeField]
    private PlayerStateMachine _playerStateMachine;

    public Weapon weapon => _weapon;

    private void Awake()
    {
        MessagingCenter.Subscribe<WeaponDialog, WeaponDialog.Clans>(this, WeaponDialog.MessageWantToChangeWeapon, (sender, clan) =>
        {
            ComboGroup comboGroup = _playerStateMachine.ComboFactory.ComboGroups[(int)clan];
            foreach (var combo in _playerStateMachine.ComboFactory.ComboGroups)
            {
                if (combo.weaponObject != null)
                    combo.weaponObject.SetActive(false);
            }

            _weapon = comboGroup.weapon;
            comboGroup.weaponObject.SetActive(true);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<WeaponDialog, WeaponDialog.Clans>(this, WeaponDialog.MessageWantToChangeWeapon);
    }

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
        var comboGroup = _playerStateMachine.ComboFactory.ComboGroups.First(x => x.name == _weapon.WeaponName);
        float damageAdjust = comboGroup.combos[currentCombo].damage * (_playerStateMachine.playerManager.soulBerserk? 2f : 1f);

        _weapon.StartDealDamage(damageAdjust, comboGroup.combos[currentCombo].eulerAngle);
    }

    public void EndDealWeaponDamage()
    {
        _weapon.EndDealDamage();
    }
}
