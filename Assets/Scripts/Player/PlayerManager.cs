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
    private UnityEvent onEvade;
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
    public bool IsImmortal { get; set; }
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

        MessagingCenter.Subscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyTakeDamage, (sender) =>
        {
            GetSoul(0.5f, true);
        });

        MessagingCenter.Subscribe<PlayerSoulState>(this, PlayerSoulState.MessageOnImmortal, (sender) =>
        {
            IsImmortal = true;
        });

        MessagingCenter.Subscribe<GameManager, GameManager.GameState>(this, GameManager.MessageOnChangedGameState, (sender, gameState) =>
        {
            if (gameState == GameManager.GameState.PAUSE)
            {
                IsImmortal = true;
            }
            else if (gameState == GameManager.GameState.GAMEPLAY)
            {
                IsImmortal = false;
            }
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<HudLoader>(this, HudLoader.MessageOnHudLoaded);
        MessagingCenter.Unsubscribe<PlayerDashState>(this, PlayerDashState.MessageOnDashStart);
        MessagingCenter.Unsubscribe<PlayerDashState>(this, PlayerDashState.MessageOnDashEnd);
        MessagingCenter.Unsubscribe<CombatTutorial>(this, CombatTutorial.MessageOnTutorialComplete);
        MessagingCenter.Unsubscribe<HealFlower>(this, HealFlower.MessageWantToRecoverPlayer);
        MessagingCenter.Unsubscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyTakeDamage);
        MessagingCenter.Unsubscribe<PlayerSoulState>(this, PlayerSoulState.MessageOnImmortal);
        MessagingCenter.Unsubscribe<GameManager, GameManager.GameState>(this, GameManager.MessageOnChangedGameState);
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
        if (IsImmortal) return;

        if (_isDash)
        {
            onEvade?.Invoke();
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

    public void InstantDead()
    {
        _hp = 0;
        MessagingCenter.Send(this, MessageOnHpChanged);
    }

    public void Heal(float value)
    {
        _hp += value;
        if (_hp > _maxHp) _hp = _maxHp;

        AudioManager.Instance.PlaySFX("Heal");
        onHeal?.Invoke();
        MessagingCenter.Send(this, MessageOnHpChanged);
    }

    public void GetSoul(float soul, bool noEffect = false)
    {
        if (!_soulBerserk)
        {
            _soul += soul;
        }    
        if (_soul > 100) _soul = 100;

        if (!noEffect)
        {
            onGetSoul?.Invoke();
            AudioManager.Instance.PlaySFX("Soul");
        }
        MessagingCenter.Send(this, MessageOnSoulChanged);
    }

    public void ActivateSoulBerserk()
    {
        IsImmortal = false;
        StartCoroutine(SoulBerserkCoroutine());
    }

    IEnumerator SoulBerserkCoroutine()
    {
        _soulBerserk = true;
        CameraShake.instance.InstantShake(0.2f);
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
