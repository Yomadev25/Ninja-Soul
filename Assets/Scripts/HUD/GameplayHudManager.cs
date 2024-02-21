using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayHudManager : MonoBehaviour
{
    public const string MessageWantToRestart = "Want To Restart";
    public const string MessageWantToExitLevel = "Want To Exit Level";

    [SerializeField]
    private Animator _animator;

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
    [SerializeField]
    private CanvasGroup[] _weaponSlots;

    [Header("Boss HP")]
    [SerializeField]
    private CanvasGroup _bossHp;
    [SerializeField]
    private Image _bossHpFill;
    [SerializeField]
    private TMP_Text _bossNameText;

    [Header("Clear Dialog")]
    [SerializeField]
    private Button _restartButton;
    [SerializeField]
    private Button _exitButton;

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

        MessagingCenter.Subscribe<EnemyManager>(this, EnemyManager.MessageOnUpdateHp, (sender) =>
        {
            if (sender.Enemy.level == Enemy.Level.BOSS || sender.Enemy.level == Enemy.Level.MINI_BOSS)
            {
                UpdateBossHp(sender.Enemy.name.ToLower(), sender.maxHp, sender.hp);
            }
        });

        MessagingCenter.Subscribe<GameManager>(this, GameManager.MessageWantToSelectWeapon, (sender) =>
        {
            InitWeaponSelectionDialog();
        });

        MessagingCenter.Subscribe<GameManager>(this, GameManager.MessageWantToDisposeWeapon, (sender) =>
        {
            DisposeWeaponSelectionDialog();
        });

        MessagingCenter.Subscribe<ComboFactory, ComboGroup[]>(this, ComboFactory.MessageSendComboData, (sender, combo) =>
        {
            UpdateWeaponList(combo);
        });

        MessagingCenter.Subscribe<WeaponDialog, WeaponDialog.Clans>(this, WeaponDialog.MessageWantToChangeWeapon, (sender, clan) =>
        {
            UpdateActivatedWeapon((int)clan);
        });

        MessagingCenter.Subscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied, (sender) =>
        {
            Eliminated();
        });

        MessagingCenter.Subscribe<GameManager>(this, GameManager.MessageOnLevelCompleted, (sender) =>
        {
            LevelClear();
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<PlayerManager>(this, PlayerManager.MessageOnHpChanged);
        MessagingCenter.Unsubscribe<PlayerManager>(this, PlayerManager.MessageOnSoulChanged);
        MessagingCenter.Unsubscribe<EnemyManager>(this, EnemyManager.MessageOnUpdateHp);
        MessagingCenter.Unsubscribe<GameManager>(this, GameManager.MessageWantToSelectWeapon);
        MessagingCenter.Unsubscribe<GameManager>(this, GameManager.MessageWantToDisposeWeapon);
        MessagingCenter.Unsubscribe<ComboFactory, ComboGroup>(this, ComboFactory.MessageSendComboData);
        MessagingCenter.Unsubscribe<WeaponDialog, WeaponDialog.Clans>(this, WeaponDialog.MessageWantToChangeWeapon);
        MessagingCenter.Unsubscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied);
        MessagingCenter.Unsubscribe<GameManager>(this, GameManager.MessageOnLevelCompleted);
    }

    private void Start()
    {
        _restartButton.onClick.AddListener(() => MessagingCenter.Send(this, MessageWantToRestart));
        _exitButton.onClick.AddListener(() => MessagingCenter.Send(this, MessageWantToExitLevel));
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

    private void UpdateBossHp(string name, float maxHp, float hp)
    {
        _bossNameText.text = name;
        if (_bossHp.alpha == 0)
        {
            _bossHp.LeanAlpha(1f, 0.5f);
        }
        
        _bossHpFill.fillAmount = hp / maxHp;
        if (hp <= 0)
        {
            _bossHp.LeanAlpha(0f, 0.5f);
        }
    }

    private void UpdateSoulGauge(float value)
    {
        _soulGaugeFill.fillAmount = value / 100;
    }

    private void UpdateWeaponList(ComboGroup[] comboGroup)
    {
        for (int i = 0; i < _weaponSlots.Length; i++)
        {
            _weaponSlots[i].alpha = comboGroup[i].isUnlocked? 1 : 0.3f;
        }
    }

    private void UpdateActivatedWeapon(int index)
    {
        _weaponSlots[index].transform.SetSiblingIndex(_weaponSlots[index].transform.parent.childCount - 1);
        if (index != 0)
            _weaponSlots[0].transform.SetSiblingIndex(_weaponSlots[index].transform.parent.childCount - 2);
    }

    private void InitWeaponSelectionDialog()
    {
        _animator.SetTrigger("Weapon");
        _animator.SetBool("isSelectWeapon", false);
    }

    private void DisposeWeaponSelectionDialog()
    {
        _animator.SetBool("isSelectWeapon", true);
    }

    private void Eliminated()
    {
        _animator.SetTrigger("Eliminated");
    }

    private void LevelClear()
    {
        _animator.SetTrigger("Clear");
    }
}
