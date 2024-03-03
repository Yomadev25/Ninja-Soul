using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TransitionManager : Singleton<TransitionManager>
{
    [Header("Transition HUD")]
    [SerializeField]
    private CanvasGroup _sceneTransition;
    [SerializeField]
    private CanvasGroup _loadingGroup;

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

    public void NormalFadeIn(float duration = 1f, UnityAction callback = null)
    {
        
    }

    public void NormalFadeOut(float duration = 1f, UnityAction callback = null)
    {
        
    }
}
