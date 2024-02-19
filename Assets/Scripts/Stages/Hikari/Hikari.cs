using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hikari : MonoBehaviour
{
    public void GoToTutorial()
    {
        TransitionManager.Instance.SceneFadeIn(1, () =>
            SceneManager.LoadScene("Tutorial"));
    }
}
