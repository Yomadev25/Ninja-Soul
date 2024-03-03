using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const string MessageOnChangedGameState = "On Changed Game State";
    public const string MessageWantToSelectWeapon = "Want To Select Weapon";
    public const string MessageWantToDisposeWeapon = "Want To Dispose Weapon";
    public const string MessageWantToPause = "Want To Pause";
    public const string MessageOnLevelCompleted = "On Level Completed";

    public enum GameState
    {
        GAMEPLAY,
        WEAPON_SELECTION,
        CUTSCENE,
        PAUSE,
        GAMEOVER,
        CLEAR
    }

    public static GameManager instance;

    [Header("Input Configurations")]
    [SerializeField]
    private InputActionReference _weaponSelectionInput;
    [SerializeField]
    private InputActionReference _pauseInput;

    private GameState _currentGameState;

    private void Awake()
    {
        instance = this;

        MessagingCenter.Subscribe<WeaponDialog, WeaponDialog.Clans>(this, WeaponDialog.MessageWantToChangeWeapon, (sender, clan) =>
        {
            DisposeWeaponSelection();
        });

        MessagingCenter.Subscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied, (sender) =>
        {
            Invoke(nameof(RestartLevel), 4f);
        });

        MessagingCenter.Subscribe<PauseHudManager>(this, PauseHudManager.MessageWantToResume, (sender) =>
        {
            Resume();
        });

        MessagingCenter.Subscribe<GameplayHudManager>(this, GameplayHudManager.MessageWantToRestart, (sender) =>
        {
            RestartLevel();
        });

        MessagingCenter.Subscribe<GameplayHudManager>(this, GameplayHudManager.MessageWantToExitLevel, (sender) =>
        {
            ExitLevel();
        });

        #region STAGE CLEAR EVENT
        MessagingCenter.Subscribe<SoulTutorial>(this, SoulTutorial.MessageOnTutorialComplete, (sender) =>
        {
            LevelComplete();
        });

        MessagingCenter.Subscribe<Genbu>(this, Genbu.MessageClearGenbuStage, (sender) =>
        {
            LevelComplete();
        });

        MessagingCenter.Subscribe<Seiryu>(this, Seiryu.MessageClearSeiryuStage, (sender) =>
        {
            LevelComplete();
        });
        #endregion
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<WeaponDialog, WeaponDialog.Clans>(this, WeaponDialog.MessageWantToChangeWeapon);
        MessagingCenter.Unsubscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied);
        MessagingCenter.Unsubscribe<PauseHudManager>(this, PauseHudManager.MessageWantToResume);
        MessagingCenter.Unsubscribe<GameplayHudManager>(this, GameplayHudManager.MessageWantToRestart);
        MessagingCenter.Unsubscribe<GameplayHudManager>(this, GameplayHudManager.MessageWantToExitLevel);
        MessagingCenter.Unsubscribe<SoulTutorial>(this, SoulTutorial.MessageOnTutorialComplete);
        MessagingCenter.Unsubscribe<Genbu>(this, Genbu.MessageClearGenbuStage);
        MessagingCenter.Unsubscribe<Seiryu>(this, Seiryu.MessageClearSeiryuStage);

        _weaponSelectionInput.action.performed -= (ctx) =>
        {
            InitWeaponSelection();
        };

        _weaponSelectionInput.action.canceled -= (ctx) =>
        {
            DisposeWeaponSelection();
        };

        _pauseInput.action.started -= (ctx) =>
        {
            Pause();
        };
    }

    private void Start()
    {
        InitKeyInput();
        TransitionManager.Instance.SceneFadeOut();
    }

    private void InitKeyInput()
    {
        _weaponSelectionInput.action.performed += (ctx) =>
        {
            InitWeaponSelection();
        };

        _weaponSelectionInput.action.canceled += (ctx) =>
        {
            DisposeWeaponSelection();
        };

        _pauseInput.action.started += (ctx) =>
        {
            Pause();
        };
    }

    private void ChangeGameState(GameState state)
    {
        if (_currentGameState == state) return;
        _currentGameState = state;

        MessagingCenter.Send(this, MessageOnChangedGameState, _currentGameState);
    }

    private void InitWeaponSelection()
    {
        if (_currentGameState != GameState.GAMEPLAY) return;
        ChangeGameState(GameState.WEAPON_SELECTION);

        MessagingCenter.Send(this, MessageWantToSelectWeapon);
    }

    private void DisposeWeaponSelection()
    {
        if (_currentGameState != GameState.WEAPON_SELECTION) return;
        ChangeGameState(GameState.GAMEPLAY);

        MessagingCenter.Send(this, MessageWantToDisposeWeapon);
    }

    private void Pause()
    {
        if (_currentGameState != GameState.GAMEPLAY) return;
        ChangeGameState(GameState.PAUSE);

        MessagingCenter.Send(this, MessageWantToPause);
    }

    private void Resume()
    {
        if (_currentGameState != GameState.PAUSE) return;
        ChangeGameState(GameState.GAMEPLAY);
    }

    private void RestartLevel()
    {
        PlayerData.Instance.hp = 10;
        PlayerData.Instance.soul = 0;
        TransitionManager.Instance.SceneFadeIn(0.5f, () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    private void ExitLevel()
    {
        PlayerData.Instance.hp = 10;
        PlayerData.Instance.soul = 0;
        TransitionManager.Instance.SceneFadeIn(0.5f, () => SceneManager.LoadScene("Hikari"));
    }

    private void LevelComplete()
    {
        ChangeGameState(GameState.CLEAR);
        PlayerData.Instance.hp = 10;
        PlayerData.Instance.soul = 0;
        MessagingCenter.Send(this, MessageOnLevelCompleted);
    }
}
