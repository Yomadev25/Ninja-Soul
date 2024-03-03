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

    public void GoToTutorial()
    {
        TransitionManager.Instance.SceneFadeIn(0.5f, () =>
            SceneManager.LoadScene("Tutorial"));
    }
}
