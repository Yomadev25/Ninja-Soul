using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField]
    private Weapon[] _weapons;
    [SerializeField]
    private PlayerStateMachine _playerStateMachine;

    public Weapon[] weapons => _weapons;

    private void Awake()
    {
        MessagingCenter.Subscribe<WeaponDialog, WeaponDialog.Clans>(this, WeaponDialog.MessageWantToChangeWeapon, (sender, clan) =>
        {
            ComboGroup comboGroup = _playerStateMachine.ComboFactory.ComboGroups[(int)clan];
            foreach (var combo in _playerStateMachine.ComboFactory.ComboGroups)
            {
                foreach (GameObject weaponObject in combo.weaponObjects)
                {
                    weaponObject.SetActive(false);
                }
            }

            _weapons = comboGroup.weapons;
            foreach (GameObject weaponObject in comboGroup.weaponObjects)
            {
                weaponObject.SetActive(true);
            }

            _playerStateMachine.Anim.runtimeAnimatorController = comboGroup.combos[0].animation;
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<WeaponDialog, WeaponDialog.Clans>(this, WeaponDialog.MessageWantToChangeWeapon);
    }

    void Start()
    {
        if (_weapons.Length == 0)
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
        var comboGroup = _playerStateMachine.ComboFactory.ComboGroups.First(x => x.name == _weapons[0].WeaponName);
        float damageAdjust = comboGroup.combos[currentCombo].damage * (_playerStateMachine.playerManager.soulBerserk? 2f : 1f);

        foreach (Weapon weapon in _weapons)
        {
            weapon.StartDealDamage(damageAdjust);
        }
    }

    public void EndDealWeaponDamage()
    {
        foreach (Weapon weapon in _weapons)
        {
            weapon.EndDealDamage();
        }
    }
}
