using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour, IDamageable
{
    [Header("Properties")]
    [SerializeField]
    private float _maxHp;
    [SerializeField]
    private float _hp;
    [SerializeField]
    private float _soul;
    [SerializeField]
    private bool _soulBerserk;

    [Header("References")]
    [SerializeField]
    private PlayerStateMachine _playerStateMachine;
    [SerializeField]
    private Animator _anim;

    [Header("Events")]
    [SerializeField]
    private UnityEvent onTakeDamage;
    [SerializeField]
    private UnityEvent onHeal;
    [SerializeField]
    private UnityEvent onGetSoul;
    [SerializeField]
    private UnityEvent onStartSoulBerserk;
    [SerializeField]
    private UnityEvent onEndedSoulBerserk;

    #region PUBLIC VARIABLES

    public float maxHp => _maxHp;
    public float hp => _hp;
    public float soul => _soul;
    public bool soulBerserk => _soulBerserk;

    #endregion

    #region MESSAGE FOR PUB/SUB

    public const string MessageOnHpChanged = "Hp Changed";
    public const string MessageOnTakeDamage = "On Take Damage";
    public const string MessageOnSoulChanged = "Soul Changed";
    public const string MessageOnPlayerDied = "Player Died";

    #endregion

    public bool IsDie { get; set; }
    public bool IsJump { get; set; }
    private bool _isDash;

    private void Awake()
    {
        if (PlayerData.Instance == null)
        {
            _hp = _maxHp;
        }
        else
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Hikari")
            {
                _hp = _maxHp;
                _soul = 0f;
            }
            else
            {
                _hp = PlayerData.Instance.hp;
                _soul = PlayerData.Instance.soul;
            }
        }

        MessagingCenter.Subscribe<HudLoader>(this, HudLoader.MessageOnHudLoaded, (sender) =>
        {
            InitPlayerHUD();
        });

        MessagingCenter.Subscribe<PlayerDashState>(this, PlayerDashState.MessageOnDashStart, (sender) =>
        {
            _isDash = true;
        });

        MessagingCenter.Subscribe<PlayerDashState>(this, PlayerDashState.MessageOnDashEnd, (sender) =>
        {
            _isDash = false;
        });

        MessagingCenter.Subscribe<CombatTutorial>(this, CombatTutorial.MessageOnTutorialComplete, (sender) =>
        {
            GetSoul(100f);
        });

        MessagingCenter.Subscribe<HealFlower>(this, HealFlower.MessageWantToRecoverPlayer, (sender) =>
        {
            Heal(_maxHp);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<HudLoader>(this, HudLoader.MessageOnHudLoaded);
        MessagingCenter.Unsubscribe<PlayerDashState>(this, PlayerDashState.MessageOnDashStart);
        MessagingCenter.Unsubscribe<PlayerDashState>(this, PlayerDashState.MessageOnDashEnd);
        MessagingCenter.Unsubscribe<CombatTutorial>(this, CombatTutorial.MessageOnTutorialComplete);
        MessagingCenter.Unsubscribe<HealFlower>(this, HealFlower.MessageWantToRecoverPlayer);
    }

    private void InitPlayerHUD()
    {
        MessagingCenter.Send(this, MessageOnHpChanged);
        MessagingCenter.Send(this, MessageOnSoulChanged);
    }

    private void Update()
    {
        if (_hp <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage, GameObject effect = null, bool impact = false)
    {
        if (IsJump) return;

        if (_isDash)
        {
            TimeStop.Instance.StopTime(0.3f, 10, 0.1f);
            return;
        }

        _hp -= damage;
        _anim.SetTrigger("Hit");

        if (effect != null)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
        }

        if (impact)
        {
            TimeStop.Instance.StopTime(0.05f, 10, 0.1f);
        }

        onTakeDamage?.Invoke();
        MessagingCenter.Send(this, MessageOnHpChanged);
        MessagingCenter.Send(this, MessageOnTakeDamage);
    }

    public void Heal(float value)
    {
        _hp += value;
        if (_hp > _maxHp) _hp = _maxHp;

        onHeal?.Invoke();
        MessagingCenter.Send(this, MessageOnHpChanged);
    }

    public void GetSoul(float soul)
    {
        if (!_soulBerserk)
        {
            _soul += soul;
        }    
        if (_soul > 100) _soul = 100;

        onGetSoul?.Invoke();
        MessagingCenter.Send(this, MessageOnSoulChanged);
    }

    public void ActivateSoulBerserk()
    {
        StartCoroutine(SoulBerserkCoroutine());
    }

    IEnumerator SoulBerserkCoroutine()
    {
        _soulBerserk = true;
        onStartSoulBerserk?.Invoke();

        while (_soul > 0)
        {
            _soul -= Time.deltaTime * 8f;
            MessagingCenter.Send(this, MessageOnSoulChanged);

            yield return null;
        }
        if (_soul < 0) _soul = 0;

        _soulBerserk = false;
        LeanTween.value(1, 0, 0.5f).setEaseInSine().setOnUpdate((value) =>
        {
            _playerStateMachine.soulVolume.weight = value;
        });
        onEndedSoulBerserk?.Invoke();
    }

    private void Die()
    {
        if (IsDie) return;

        IsDie = true;
        _playerStateMachine.enabled = false;

        _anim.applyRootMotion = true;
        _anim.SetTrigger("Die");

        PlayerData.Instance.hp = _maxHp;
        MessagingCenter.Send(this, MessageOnPlayerDied);
    }
}
