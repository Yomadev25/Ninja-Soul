using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComboFactory : MonoBehaviour
{
    public const string MessageSendComboData = "Send Combo Data";

    [SerializeField]
    private ComboGroup[] _comboGroups;
    public ComboGroup[] ComboGroups => _comboGroups;

    private void Awake()
    {
        MessagingCenter.Subscribe<GameManager>(this, GameManager.MessageWantToSelectWeapon, (sender) =>
        {
            MessagingCenter.Send(this, MessageSendComboData, _comboGroups);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<GameManager>(this, GameManager.MessageWantToSelectWeapon);
    }

    private void Start()
    {
        MessagingCenter.Send(this, MessageSendComboData, _comboGroups);
    }

    private void UnlockWeapon(string name)
    {
        _comboGroups.First(x => x.name == name).isUnlocked = true;
        MessagingCenter.Send(this, MessageSendComboData, _comboGroups);
    }
}

[System.Serializable]
public class ComboGroup
{
    public string name;
    public GameObject weaponObject;
    public Weapon weapon;
    public Combo[] combos;
    public bool isUnlocked;
}
