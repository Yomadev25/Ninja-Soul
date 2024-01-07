using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        MessagingCenter.Subscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied, (sender) =>
        {
            Invoke(nameof(RestartLevel), 5f);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied);
    }

    private void Start()
    {
        TransitionManager.Instance.SceneFadeOut();
    }

    private void RestartLevel()
    {
        TransitionManager.Instance.SceneFadeIn(1f, () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }
}
