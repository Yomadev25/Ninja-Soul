using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Cutscene1 : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM("Hikari");
        TransitionManager.Instance.SceneFadeOut();
    }
}
