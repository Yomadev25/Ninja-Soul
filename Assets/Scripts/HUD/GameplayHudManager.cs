using System.Collections;
using System.Collections.Generic;
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

    private void UpdateSoulGauge(float value)
    {
        _soulGaugeFill.fillAmount = value / 100;
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
