using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour, IDamageable
{
    public const string MessageOnEnemyAppeared = "On Enemy Appeared";
    public const string MessageOnUpdateHp = "On Update Hp";
    public const string MessageOnEnemyDead = "On Enemy Dead";
    public const string MessageOnEnemyTakeDamage = "On Enemy Take Damage";

    [Header("Enemy Profile")]
    [SerializeField]
    private Enemy _enemy;
    [SerializeField]
    private EnemyStateMachine _enemyStateMachine;

    [Header("Properties")]
    [SerializeField]
    private float _maxHp;
    [SerializeField]
    private float _hp;

    [Header("References")]
    [SerializeField]
    private Animator _anim;
    private Collider _collider;

    [Header("HUD")]
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Image _hpFill;
    [SerializeField]
    private float _canvasDuration = 10;
    private float _currentCanvasDuration;

    [Header("Sound Effects")]
    [SerializeField]
    private string _hitSfx;

    [Header("Events")]
    [SerializeField]
    private UnityEvent onTakeDamage;
    [SerializeField]
    private UnityEvent onHeal;
    [SerializeField]
    private UnityEvent onDead;

    bool isDie;

    #region PUBLIC VARIABLES
    public Enemy Enemy => _enemy;
    public EnemyStateMachine stateMachine => _enemyStateMachine;
    public float hp => _hp;
    public float maxHp => _maxHp;
    #endregion

    private void Start()
    {
        _hp = _maxHp;
        _collider = GetComponent<Collider>();
        MessagingCenter.Send(this, MessageOnUpdateHp);
        MessagingCenter.Send(this, MessageOnEnemyAppeared);
    }

    private void Update()
    {
        if (_hp <= 0)
        {
            Die();
        }
        else
        {
            if (_currentCanvasDuration > 0)
            {
                _currentCanvasDuration -= Time.deltaTime;
            }
            else
            {
                if (_canvasGroup != null)
                    _canvasGroup.alpha = 0;
            }
        }
    }

    public void TakeDamage(float damage, GameObject effect = null, bool impact = false)
    {
        if (isDie) return;

        if (_enemyStateMachine.GetVisibleTarget() == null && _enemy.assasinate)
        {
            damage += 9999;
        }

        if (impact)
        {
            TimeStop.Instance.StopTime(0.05f, 10, 0.1f);
        }
        else
        {
            if (damage >= _hp)
            {
                TimeStop.Instance.StopTime(0.05f, 10, 0.1f);
                AudioManager.Instance.PlaySFX("Kill");

                Destroy(EffectManager.Instance.Spawn("Kill Impact", transform.position + Vector3.up, Quaternion.identity), 1f);

                if (_enemy.name != "Scarecrow")
                {
                    GameObject blood = EffectManager.Instance.Spawn("Kill", transform.position, Quaternion.identity);
                    blood.transform.parent = transform;
                    blood.transform.localPosition = Vector3.zero + Vector3.up;
                    blood.transform.localEulerAngles = Vector3.zero;
                    Destroy(blood, 1.5f);
                }
                
            }
        }

        _hp -= damage;
        _anim.SetTrigger("Hit");
        AudioManager.Instance.PlaySFX(_hitSfx);

        if (effect != null)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
        }

        UpdateHpBar();
        onTakeDamage?.Invoke();
        MessagingCenter.Send(this, MessageOnEnemyTakeDamage);
        MessagingCenter.Send(this, MessageOnUpdateHp);
    }

    private void UpdateHpBar()
    {
        if (_enemy.level == Enemy.Level.BOSS) return;
        if (_canvasGroup == null) return;

        _currentCanvasDuration = _canvasDuration;
        _hpFill.fillAmount = _hp / _maxHp;

        if (LeanTween.isTweening(_canvasGroup.gameObject))
        {
            LeanTween.cancel(_canvasGroup.gameObject);
        }

        _canvasGroup.LeanAlpha(1, 0.2f).setOnComplete(() =>
        {
            _canvasGroup.LeanAlpha(0, 1f).setDelay(_canvasDuration - 1.2f);
        });
    }

    private void Die()
    {
        if (isDie) return;
        isDie = true;
 
        _enemyStateMachine.enabled = false;
        _anim.applyRootMotion = false;
        _anim.SetLayerWeight(1, 0);
        _anim.SetTrigger("Die");
        _collider.enabled = false;
        if (_canvasGroup != null)
            _canvasGroup.LeanAlpha(0, 1f);

        if (EffectManager.Instance != null)
        {
            EffectManager.Instance.Spawn("Soul", this.transform.position, Quaternion.identity);
        }

        onDead?.Invoke();
        MessagingCenter.Send(this, MessageOnEnemyDead);
        Destroy(this.gameObject, 4f);
    }
}
