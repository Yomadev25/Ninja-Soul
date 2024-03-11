using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StageIntro : MonoBehaviour
{
    public const string MessageWantToPlayIntro = "Want To Play Intro";
    public const string MessageIntroEnded = "Intro Ended";

    [SerializeField]
    private PlayableDirector _director;
    [SerializeField]
    private Camera _camera;

    private bool _isOver;

    private void Update()
    {
        if (_director.time >= _director.duration)
        {
            if (_isOver) return;
            _isOver = true;
            TransitionManager.Instance.NormalFadeIn(0.5f, () =>
            {
                _camera.orthographic = true;
                MessagingCenter.Send(this, MessageIntroEnded);
                TransitionManager.Instance.NormalFadeOut();
                gameObject.SetActive(false);
            });
        }
    }

    public void PlayIntro()
    {
        _camera.orthographic = false;
        _director.Play();

        MessagingCenter.Send(this, MessageWantToPlayIntro);
    }
}
