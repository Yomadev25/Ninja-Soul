using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHud : MonoBehaviour
{
    [Header("Weapon Group")]
    [SerializeField]
    private CanvasGroup[] _weaponButtons;

    [Header("Inspect Panel")]
    [SerializeField]
    private Image _weaponIcon;
    [SerializeField]
    private TMP_Text _nameText;
    [SerializeField]
    private TMP_Text _descriptionText;
    [SerializeField]
    private Transform _modelTransform;

    private void Awake()
    {
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
            bool isUnlock = comboGroup[i].isUnlocked;
            _weaponButtons[i].interactable = isUnlock;
            _weaponButtons[i].blocksRaycasts = isUnlock;

            GameObject lockIcon = _weaponButtons[i].transform.Find("Lock").gameObject;
            GameObject weaponIcon = _weaponButtons[i].transform.Find("(image) weapon").gameObject;

            if (isUnlock)
            {
                _weaponButtons[i].alpha = 1;
                lockIcon.SetActive(false);
                weaponIcon.SetActive(true);

                ComboGroup combo = comboGroup[i];
                Button button = _weaponButtons[i].GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    SelectWeapon(combo);
                });
            }
            else
            {
                _weaponButtons[i].alpha = 0.5f;
                lockIcon.SetActive(true);
                weaponIcon.SetActive(false);
            }         
        }

        SelectWeapon(comboGroup[0]);
    }

    private void SelectWeapon(ComboGroup combo)
    {
        _weaponIcon.sprite = combo.icon;
        _weaponIcon.SetNativeSize();
        _nameText.text = combo.name;
        _descriptionText.text = combo.description;

        foreach (Transform model in _modelTransform)
        {
            Destroy(model.gameObject);
        }
        GameObject Go = Instantiate(combo.weaponObject, _modelTransform);
        Go.layer = LayerMask.NameToLayer("Inspect");

        foreach (Transform item in Go.transform)
        {
            item.gameObject.layer = LayerMask.NameToLayer("Inspect");
        }

        Go.SetActive(true);
    }
}
