using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    private void Start()
    {
        TransitionManager.Instance.SceneFadeOut();
        AudioManager.Instance.PlayBGM("Ending");
    }

    public void Complete()
    {
        PlayerData.Instance.GetPlayerData().completed = true;

        Player player = PlayerData.Instance.GetPlayerData();
        player.lastDate = DateTime.Now;
        SaveManager.Instance.Save(player);

        AudioManager.Instance.StopBGM();
        //Show popup
    }

    public void GoToMenu()
    {
        TransitionManager.Instance.SceneFadeIn(0.5f, () =>
        {
            SceneManager.LoadScene("Menu");
        });
    }
}
