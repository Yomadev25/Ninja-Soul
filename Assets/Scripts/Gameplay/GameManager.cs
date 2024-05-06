using System;
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
    public GameState currentGameState => _currentGameState;

    private void Awake()
    {
        instance = this;

        MessagingCenter.Subscribe<WeaponDialog, WeaponDialog.Clans>(this, WeaponDialog.MessageWantToChangeWeapon, (sender, clan) =>
        {
            DisposeWeaponSelection();
        });

        MessagingCenter.Subscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied, (sender) =>
        {
            AudioManager.Instance.StopBGM();
            AudioManager.Instance.PlaySFX("Eliminated");
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

        MessagingCenter.Subscribe<DialogueManager, Dialogue>(this, DialogueManager.MessageOnActivatedDialogue, (sender, dialogue) =>
        {
            ChangeGameState(GameState.CUTSCENE);
        });

        MessagingCenter.Subscribe<DialogueManager, Dialogue>(this, DialogueManager.MessageOnDialogueEnded, (sender, dialogue) =>
        {
            ChangeGameState(GameState.GAMEPLAY);
        });

        MessagingCenter.Subscribe<StageIntro>(this, StageIntro.MessageWantToPlayIntro, (sender) =>
        {
            ChangeGameState(GameState.CUTSCENE);
        });

        MessagingCenter.Subscribe<StageIntro>(this, StageIntro.MessageIntroEnded, (sender) =>
        {
            ChangeGameState(GameState.GAMEPLAY);
        });

        #region STAGE CLEAR EVENT
        MessagingCenter.Subscribe<TutorialManager>(this, TutorialManager.MessageOnTutorialComplete, (sender) =>
        {
            PlayerData.Instance.GetPlayerData().tutorial = true;
            Invoke(nameof(LevelComplete), 1f);
        });

        MessagingCenter.Subscribe<Genbu>(this, Genbu.MessageClearGenbuStage, (sender) =>
        {
            PlayerData.Instance.GetPlayerData().genbu = true;
            PlayerData.Instance.GetPlayerData().knuckles = true;
            Invoke(nameof(LevelComplete), 1f);
        });

        MessagingCenter.Subscribe<Seiryu>(this, Seiryu.MessageClearSeiryuStage, (sender) =>
        {
            PlayerData.Instance.GetPlayerData().seiryu = true;
            PlayerData.Instance.GetPlayerData().jevalin = true;
            Invoke(nameof(LevelComplete), 1f);
        });

        MessagingCenter.Subscribe<Byakko>(this, Byakko.MessageClearByakkoStage, (sender) =>
        {
            PlayerData.Instance.GetPlayerData().byakko = true;
            PlayerData.Instance.GetPlayerData().sword = true;
            Invoke(nameof(LevelComplete), 1f);
        });

        MessagingCenter.Subscribe<Suzaku>(this, Suzaku.MessageClearSuzakuStage, (sender) =>
        {
            PlayerData.Instance.GetPlayerData().suzaku = true;
            PlayerData.Instance.GetPlayerData().sickles = true;
            Invoke(nameof(LevelComplete), 1f);
        });

        MessagingCenter.Subscribe<AnotherShin>(this, AnotherShin.MessageClearLastStage, (sender) =>
        {
            Invoke(nameof(LevelComplete), 1f);
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
        MessagingCenter.Unsubscribe<DialogueManager, Dialogue>(this, DialogueManager.MessageOnActivatedDialogue);
        MessagingCenter.Unsubscribe<DialogueManager, Dialogue>(this, DialogueManager.MessageOnDialogueEnded);
        MessagingCenter.Unsubscribe<StageIntro>(this, StageIntro.MessageWantToPlayIntro);
        MessagingCenter.Unsubscribe<StageIntro>(this, StageIntro.MessageIntroEnded);

        MessagingCenter.Unsubscribe<TutorialManager>(this, TutorialManager.MessageOnTutorialComplete);
        MessagingCenter.Unsubscribe<Genbu>(this, Genbu.MessageClearGenbuStage);
        MessagingCenter.Unsubscribe<Seiryu>(this, Seiryu.MessageClearSeiryuStage);
        MessagingCenter.Unsubscribe<Byakko>(this, Byakko.MessageClearByakkoStage);
        MessagingCenter.Unsubscribe<Suzaku>(this, Suzaku.MessageClearSuzakuStage);
        MessagingCenter.Unsubscribe<AnotherShin>(this, AnotherShin.MessageClearLastStage);

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
        string scene = SceneManager.GetActiveScene().name == "Final" ? "End_Cutscene" : "Hikari";
        TransitionManager.Instance.SceneFadeIn(0.5f, () => SceneManager.LoadScene(scene));
    }

    private void LevelComplete()
    {
        ChangeGameState(GameState.CLEAR);
        AudioManager.Instance.PlayBGM("Result");
        PlayerData.Instance.hp = 10;
        PlayerData.Instance.soul = 0;

        Player player = PlayerData.Instance.GetPlayerData();
        player.id = 0;
        player.lastDate = DateTime.Now;
        SaveManager.Instance.Save(player);

        MessagingCenter.Send(this, MessageOnLevelCompleted);
    }
}
