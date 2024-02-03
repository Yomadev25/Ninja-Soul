using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const string MessageOnLevelCompleted = "On Level Completed";

    public static GameManager instance;

    private void Awake()
    {
        instance = this;

        MessagingCenter.Subscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied, (sender) =>
        {
            Invoke(nameof(RestartLevel), 5f);
        });

        MessagingCenter.Subscribe<GameplayHudManager>(this, GameplayHudManager.MessageWantToRestart, (sender) =>
        {
            RestartLevel();
        });

        MessagingCenter.Subscribe<GameplayHudManager>(this, GameplayHudManager.MessageWantToExitLevel, (sender) =>
        {
            ExitLevel();
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied);
        MessagingCenter.Unsubscribe<GameplayHudManager>(this, GameplayHudManager.MessageWantToRestart);
        MessagingCenter.Unsubscribe<GameplayHudManager>(this, GameplayHudManager.MessageWantToExitLevel);
    }

    private void Start()
    {
        TransitionManager.Instance.SceneFadeOut();
    }

    private void RestartLevel()
    {
        TransitionManager.Instance.SceneFadeIn(1f, () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    private void ExitLevel()
    {
        TransitionManager.Instance.SceneFadeIn(1f, () => SceneManager.LoadScene("Hikari"));
    }

    public void LevelComplete()
    {
        MessagingCenter.Send(this, MessageOnLevelCompleted);
    }
}
