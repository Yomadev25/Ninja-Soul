using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hikari : MonoBehaviour
{
    private void Awake()
    {
        PlayerData.Instance.SetSpawnPoint(default);
    }

    private void Start()
    {
        if (PlayerData.Instance.IsCompleteAllState() && !PlayerData.Instance.GetPlayerData().completed)
        {
            TransitionManager.Instance.SceneFadeIn(0.5f, () =>
                SceneManager.LoadScene("F_Cutscene 1"));
        }
        else
        {
            AudioManager.Instance.PlayBGM("Hikari");
        }
    }

    public void GoToTutorial()
    {
        TransitionManager.Instance.SceneFadeIn(0.5f, () =>
            SceneManager.LoadScene("Tutorial"));
    }
}
