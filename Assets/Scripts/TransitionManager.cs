using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ModularMotion;

public class TransitionManager : Singleton<TransitionManager>
{
    [Header("Scene Transition HUD")]
    [SerializeField]
    private UIMotion _fadeIn;
    [SerializeField]
    private UIMotion _fadeOut;

    private void Start()
    {
        SceneFadeIn();
    }

    public void SceneFadeIn()
    {
        _fadeIn.Play();
    }

    public void SceneFadeOut()
    {
        _fadeOut.Play();
    }

    public void NormalFadeIn()
    {

    }

    public void NormalFadeOut()
    {

    }
}
