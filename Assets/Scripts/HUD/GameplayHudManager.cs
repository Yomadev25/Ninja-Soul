using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayHudManager : MonoBehaviour
{
    [Header("Player Dialog")]
    [SerializeField]
    private Image _hpTemplate;
    [SerializeField]
    private Sprite _hpSprite;
    [SerializeField]
    private Transform _hpIconRoot;
    [SerializeField]
    private int _maxHpIcon;
    [SerializeField]
    private Image _soulGaugeFill;

    private void Awake()
    {
        MessagingCenter.Subscribe<PlayerManager>(this, PlayerManager.MessageOnHpChanged, (sender) =>
        {
            UpdateHpIcon((int)sender.maxHp, (int)sender.hp);
        });

        MessagingCenter.Subscribe<PlayerManager>(this, PlayerManager.MessageOnSoulChanged, (sender) =>
        {
            UpdateSoulGauge(sender.soul);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<PlayerManager>(this, PlayerManager.MessageOnHpChanged);
    }

    private void UpdateHpIcon(int maxHp, int hp)
    {
        foreach (Transform _hp in _hpIconRoot)
        {
            Destroy(_hp.gameObject);
        }

        for (int i = 0; i < maxHp; i++)
        {
            Image hpIcon = Instantiate(_hpTemplate, _hpIconRoot);

            if (i < hp)
            {
                hpIcon.sprite = _hpSprite;
            }

            hpIcon.gameObject.SetActive(true);
        }
    }

    private void UpdateSoulGauge(float value)
    {
        _soulGaugeFill.fillAmount = value / 100;
    }
}
