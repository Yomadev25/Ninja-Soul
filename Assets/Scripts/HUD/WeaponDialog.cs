using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDialog : MonoBehaviour
{
    public const string MessageWantToChangeWeapon = "Want To Change Weapon";

    public enum Clans
    {
        Hikari,
        Byakko,
        Suzaku,
        Seiryu,
        Genbu
    }

    [SerializeField]
    private CanvasGroup[] _weaponButtons;

    private void Awake()
    {
        for (int i = 0; i < _weaponButtons.Length; i++)
        {
            int index = i;
            _weaponButtons[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectWeapon(index);
            });
        }

        MessagingCenter.Subscribe<ComboFactory, ComboGroup[]>(this, ComboFactory.MessageSendComboData, (sender, combo) =>
        {
            UpdateWeaponList(combo);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<ComboFactory, ComboGroup>(this, ComboFactory.MessageSendComboData);
    }

    private void UpdateWeaponList(ComboGroup[] comboGroup)
    {
        for (int i = 0; i < _weaponButtons.Length; i++)
        {
            _weaponButtons[i].interactable = comboGroup[i].isUnlocked;
            _weaponButtons[i].blocksRaycasts = comboGroup[i].isUnlocked;

            GameObject lockIcon = _weaponButtons[i].transform.Find("Lock").gameObject;
            GameObject weaponIcon = _weaponButtons[i].transform.Find("(image) weapon").gameObject;

            if (comboGroup[i].isUnlocked)
            {
                lockIcon.SetActive(false);
                weaponIcon.SetActive(true);
            }
            else
            {
                lockIcon.SetActive(true);
                weaponIcon.SetActive(false);
            }
        }
    }

    private void SelectWeapon(int index)
    {
        Clans clan = (Clans)index;
        MessagingCenter.Send(this, MessageWantToChangeWeapon, clan);
    }
}
