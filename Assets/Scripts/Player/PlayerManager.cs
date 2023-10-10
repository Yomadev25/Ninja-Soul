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
    public const string MessageOnSoulChanged = "Soul Changed";
    public const string MessageOnPlayerDied = "Player Died";

    #endregion

    public bool IsDie { get; set; }

    private void Awake()
    {
        _hp = _maxHp;

        MessagingCenter.Subscribe<HudLoader>(this, HudLoader.MessageOnHudLoaded, (sender) =>
        {
            InitPlayerHUD();
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<HudLoader>(this, HudLoader.MessageOnHudLoaded);
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

    public void TakeDamage(float damage)
    {
        _hp -= damage;
        _anim.SetTrigger("Hit");

        onTakeDamage?.Invoke();
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
            _soul -= Time.deltaTime * 5;
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

        MessagingCenter.Send(this, MessageOnPlayerDied);
    }
}
