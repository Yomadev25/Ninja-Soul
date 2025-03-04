using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TransitionManager : Singleton<TransitionManager>
{
    [Header("Scene Transition HUD")]
    [SerializeField]
    private CanvasGroup _sceneTransition;
    [SerializeField]
    private CanvasGroup _loadingGroup;

    [Header("Transition HUD")]
    [SerializeField]
    private CanvasGroup _transition;

    public void SceneFadeIn(float duration = 0.5f, UnityAction callback = null)
    {
        _sceneTransition.alpha = 0f;
        _sceneTransition.blocksRaycasts = true;
        _loadingGroup.alpha = 0f;

        if (LeanTween.isTweening(_sceneTransition.gameObject))
        {
            LeanTween.cancel(_sceneTransition.gameObject);
        }
        _sceneTransition.LeanAlpha(1, duration).setOnComplete(() =>
        {
            _loadingGroup.LeanAlpha(1f, 0.5f).setOnComplete(() =>
            {
                callback?.Invoke();
            });            
        });

        AudioManager.Instance.StopBGM();
    }

    public void SceneFadeOut(float duration = 0.5f, UnityAction callback = null)
    {
        _sceneTransition.alpha = 1f;
        _sceneTransition.blocksRaycasts = false;

        if (LeanTween.isTweening(_sceneTransition.gameObject))
        {
            LeanTween.cancel(_sceneTransition.gameObject);
        }
        _sceneTransition.LeanAlpha(0, duration).setDelay(0.5f).setOnComplete(() => callback?.Invoke());
    }

    #region NORMAL TRANSITION
    public void NormalFadeIn(float duration = 0.5f, UnityAction callback = null)
    {
        _transition.alpha = 0f;
        _transition.blocksRaycasts = true;
        _loadingGroup.alpha = 0f;

        if (LeanTween.isTweening(_transition.gameObject))
        {
            LeanTween.cancel(_transition.gameObject);
        }
        _transition.LeanAlpha(1, duration).setOnComplete(() =>
        {
            _loadingGroup.LeanAlpha(1f, 0.5f).setOnComplete(() =>
            {
                callback?.Invoke();
            });
        });
    }

    public void NormalFadeOut(float duration = 0.5f, UnityAction callback = null)
    {
        _transition.alpha = 1f;
        _transition.blocksRaycasts = false;

        if (LeanTween.isTweening(_transition.gameObject))
        {
            LeanTween.cancel(_transition.gameObject);
        }
        _transition.LeanAlpha(0, duration).setDelay(0.5f).setOnComplete(() => callback?.Invoke());
    }
    #endregion
}
