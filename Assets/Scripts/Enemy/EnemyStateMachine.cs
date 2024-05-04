using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyManager), typeof(NavMeshAgent))]
public class EnemyStateMachine : MonoBehaviour
{
    public const string MessageOnKnockdown = "On Knockdown";
    public const string MessageOnStandUp = "On Stand Up";

    [Header("Properties")]
    [SerializeField]
    private Enemy _enemy;
    [SerializeField]
    private Weapon _weapon;
    [SerializeField]
    private float _delayPerCombo;
    private float _currentCooldown;
    private bool _isKO;

    [Header("Field Of View")]
    [SerializeField]
    private float _viewRadius;
    [SerializeField]
    private float _chaseRadius;
    [SerializeField]
    private float _combatRadius;
    [SerializeField]
    private float _viewAngle;
    [SerializeField]
    private LayerMask _targetLayer;

    [Header("References")]
    [SerializeField]
    private NavMeshAgent _navMesh;
    [SerializeField]
    private Animator _anim;


    #region PUBLIC REFERENCES
    public EnemyBaseState CurrentState { get; set; }
    public EnemyStateFactory State { get; set; }

    public Enemy Enemy => _enemy;
    public NavMeshAgent NavMesh => _navMesh;
    public Animator Anim => _anim;

    public float CurrentCooldown => _currentCooldown;
    public bool IsReadyToCombat { get; set; }
    public int ComboCount { get; set; }
    #endregion


    private void Start()
    {
        InitializeEnemy();

        State = new EnemyStateFactory(this);
        CurrentState = State.Idle();
        CurrentState.Enter();
    }

    private void InitializeEnemy()
    {
        if (_enemy == null)
        {
            Debug.LogErrorFormat("{0} doesn't has enemy property.", this.gameObject.name);
            return;
        }

        _delayPerCombo = _enemy.delayPerCombo;
        _currentCooldown = _delayPerCombo;

        _viewRadius = _enemy.viewRadius;
        _chaseRadius = _enemy.chaseRadius;
        _viewAngle = _enemy.viewAngle;
    }

    private void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }

        CombatCooldownHandler();
    }

    private void FixedUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.FixedUpdate();
        }
    }

    public void DestroyGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    #region ENEMY VISUALIZATION
    public Transform GetVisibleTarget()
    {
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, _viewRadius, _targetLayer);

        foreach (Collider collider in targetInViewRadius)
        {
            Transform target = collider.transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (collider.transform.GetComponent<PlayerManager>().IsDie)
            {
                return null;
            }
                
            if (Vector3.Angle(transform.forward, dirToTarget) < _viewAngle / 2)
            {
                return target;
            }
        }

        return null;
    }

    public Transform GetChasedTarget()
    {
        Collider[] targetInChaseRadius = Physics.OverlapSphere(transform.position, _chaseRadius, _targetLayer);

        foreach (Collider collider in targetInChaseRadius)
        {
            if (collider.transform.GetComponent<PlayerManager>().IsDie)
                return null;
            else
                return collider.transform;
        }

        return null;
    }

    public Transform GetCombatTarget()
    {
        Collider[] targetInCombatRadius = Physics.OverlapSphere(transform.position, _combatRadius, _targetLayer);

        foreach (Collider collider in targetInCombatRadius)
        {
            if (collider.transform.GetComponent<PlayerManager>().IsDie)
                return null;
            else
                return collider.transform;
        }

        return null;
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public Vector3 TargetOffset(Transform target)
    {
        Vector3 position;
        position = target.position;
        return Vector3.MoveTowards(position, transform.position, .95f);
    }
    #endregion

    #region COMBAT HANDLER

    public void StartDealWeaponDamage()
    {
        float damageAdjust = _enemy.combos[ComboCount].damage;
        _weapon.StartDealDamage(damageAdjust);
    }

    public void EndDealWeaponDamage()
    {
        _weapon.EndDealDamage();
    }

    private void CombatCooldownHandler()
    {
        if (_currentCooldown <= 0 && !IsReadyToCombat)
        {
            IsReadyToCombat = true;
        }
        else
        {
            _currentCooldown -= Time.deltaTime;
        }
    }

    public void SetCombatRadius(float range)
    {
        _combatRadius = range;
    }

    public void ResetCombatCooldown(float cooldown = 0)
    {
        if (cooldown != 0)
        {
            _delayPerCombo = cooldown;
        }

        _currentCooldown = _delayPerCombo;
        IsReadyToCombat = false;
    }

    public void SetWeapon(Weapon weapon)
    {
        _weapon = weapon;
    }

    public void Knockdown()
    {
        if (!_isKO)
        {
            StartCoroutine(KnockdownCoroutine());
        }
    }

    IEnumerator KnockdownCoroutine()
    {
        Debug.Log("Knocked");
        _isKO = true;
        MessagingCenter.Send(this, MessageOnKnockdown);

        yield return new WaitForSeconds(_enemy.knockDuration);

        MessagingCenter.Send(this, MessageOnStandUp);
        _isKO = false;
    }

    #endregion

    #region DEBUGING
    private void OnDrawGizmosSelected()
    {
        if (_enemy == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _enemy.viewRadius);

        Vector3 viewAngleA = DirFromAngle(-_enemy.viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(_enemy.viewAngle / 2, false);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * _enemy.viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * _enemy.viewRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _enemy.chaseRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _combatRadius);
    }
    #endregion
}

public class EnemyStateFactory
{
    EnemyStateMachine _context;

    public EnemyStateFactory(EnemyStateMachine currentContext)
    {
        _context = currentContext;
    }

    public EnemyIdleState Idle()
    {
        return new EnemyIdleState(_context);
    }

    public EnemyChaseState Chase()
    {
        return new EnemyChaseState(_context);
    }

    public EnemyPrepareState Prepare() //Prepare to combat
    {
        return new EnemyPrepareState(_context);
    }

    public EnemyCombatState Combat()
    {
        return new EnemyCombatState(_context);
    }

    public EnemyKnockState Knock()
    {
        return new EnemyKnockState(_context);
    }
}
