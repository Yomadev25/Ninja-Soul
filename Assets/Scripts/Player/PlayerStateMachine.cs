using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Movement Setting")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _sprintSpeed;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _dashSpeed;

    [Header("Slope Detection")]
    [SerializeField]
    private float _playerHeight;
    [SerializeField]
    private LayerMask _groundLayer;

    [Header("Combat Setting")]
    [SerializeField]
    private ComboFactory _comboFactory;

    [Header("Input Setting")]
    [SerializeField]
    private InputActionReference _movementInput;
    [SerializeField]
    private InputActionReference _sprintInput;
    [SerializeField]
    private InputActionReference _dashInput;
    [SerializeField]
    private InputActionReference _combatInput;
    [SerializeField]
    private InputActionReference _soulInput;

    [Header("Effects")]
    [SerializeField]
    private Volume _soulVolume;

    [Header("References")]
    [SerializeField]
    private PlayerManager _playerManager;
    [SerializeField]
    private Rigidbody _rigidBody;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private PlayerEquipment _playerEquipment;
    

    #region PUBLIC REFERENCES
    public PlayerBaseState CurrentState { get; set; }
    public PlayerStateFactory State { get; set; }
    public PlayerManager playerManager => _playerManager;
    public PlayerEquipment playerEquipment => _playerEquipment;

    public Rigidbody rigidBody => _rigidBody;
    public Animator Anim => _anim;

    public ComboFactory ComboFactory => _comboFactory;
    public int ComboCount { get; set; }

    public Vector3 AxisInput { get; set; }
    public float MoveSpeed { get; set; }
    public float Speed => _speed;
    public float SprintSpeed => _sprintSpeed;
    public float DashSpeed => _dashSpeed;
    public bool CanRotate { get; set; }

    public bool PressedMove { get; set; }
    public bool PressedSprint { get; set; }
    public bool PressedDash { get; set; }
    public bool PressedCombat { get; set; }
    public bool PressedSoul { get; set; }

    public Volume soulVolume => _soulVolume;
    public Coroutine DashCoroutine { get; set; }
    #endregion

    private void Start()
    {
        State = new PlayerStateFactory(this);
        CurrentState = State.Idle();
        CurrentState.Enter();
        CanRotate = true;

        GatherInput();

        Vector3 spawnPoint = PlayerData.Instance.spawnPoint;
        if (spawnPoint != default)
        {
            transform.position = spawnPoint;
        }
    }

    private void Update()
    {
        Rotate();

        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }

    private void FixedUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.FixedUpdate();
        }
    }

    private void GatherInput()
    {
        if (_movementInput != null)
        {
            _movementInput.action.performed += (ctx) =>
            {
                if (GameManager.instance.currentGameState != GameManager.GameState.GAMEPLAY) return;

                Vector2 input = ctx.ReadValue<Vector2>();
                AxisInput = new Vector3(input.x, 0f, input.y);

                PressedMove = true;
            };

            _movementInput.action.canceled += (ctx) =>
            {
                AxisInput = Vector3.zero;
                PressedMove = false;
            };
        }
        else
        {
            Debug.LogErrorFormat("{0} isn't exist in {1}.", nameof(InputActionReference), this.name);
        }

        if (_sprintInput != null)
        {
            _sprintInput.action.performed += (ctx) =>
            {
                if (GameManager.instance.currentGameState != GameManager.GameState.GAMEPLAY) return;
                PressedSprint = true;
            };

            _sprintInput.action.canceled += (ctx) =>
            {
                PressedSprint = false;
            };
        }
        else
        {
            Debug.LogErrorFormat("{0} isn't exist in {1}.", nameof(InputActionReference), this.name);
        }

        if (_dashInput != null)
        {
            _dashInput.action.started += (ctx) =>
            {
                if (GameManager.instance.currentGameState != GameManager.GameState.GAMEPLAY) return;
                PressedDash = true;
            };

            _dashInput.action.canceled += (ctx) =>
            {
                PressedDash = false;
            };
        }
        else
        {
            Debug.LogErrorFormat("{0} isn't exist in {1}.", nameof(InputActionReference), this.name);
        }

        if (_combatInput != null)
        {
            _combatInput.action.started += (ctx) =>
            {
                if (GameManager.instance.currentGameState != GameManager.GameState.GAMEPLAY) return;
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
                PressedCombat = true;
            };

            _combatInput.action.canceled += (ctx) =>
            {
                PressedCombat = false;
            };
        }
        else
        {
            Debug.LogErrorFormat("{0} isn't exist in {1}.", nameof(InputActionReference), this.name);
        }

        if (_soulInput != null)
        {          
            _soulInput.action.started += (ctx) =>
            {
                if (GameManager.instance.currentGameState != GameManager.GameState.GAMEPLAY) return;
                PressedSoul = true;
            };

            _soulInput.action.canceled += (ctx) =>
            {
                PressedSoul = false;
            };
        }
        else
        {
            Debug.LogErrorFormat("{0} isn't exist in {1}.", nameof(InputActionReference), this.name);
        }
    }

    private void Rotate()
    {
        if (AxisInput == Vector3.zero) return;
        if (!CanRotate) return;

        var rot = Quaternion.LookRotation(Isometric.ToIso(AxisInput), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _rotateSpeed * Time.deltaTime);
    }

    public bool SoulReady()
    {
        return _playerManager.soul == 100;
    }
}

public static class Isometric
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}

public class PlayerStateFactory
{
    PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerIdleState Idle()
    {
        return new PlayerIdleState(_context);
    }

    public PlayerWalkState Walk()
    {
        return new PlayerWalkState(_context);
    }

    public PlayerRunState Run()
    {
        return new PlayerRunState(_context);
    }

    public PlayerDashState Dash()
    {
        return new PlayerDashState(_context);
    }

    public PlayerCombatState Combat()
    {
        return new PlayerCombatState(_context);
    }

    public PlayerSoulState Soul()
    {
        return new PlayerSoulState(_context);
    }
}
