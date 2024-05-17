using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Trailer : MonoBehaviour
{
    public VideoPlayer video;

    public bool isMenu;
    public int idleTimeSetting;
    float _lastIdleTime;

    bool isFade;

    void Awake()
    {
        _lastIdleTime = Time.time;
    }

    private void Start()
    {
        if (isMenu) return;
        TransitionManager.Instance.NormalFadeOut();
        if(video != null)
        {
            video.loopPointReached += ReturnToMenu;
        }
    }

    private void OnDestroy()
    {
        if (video != null)
        {
            video.loopPointReached -= ReturnToMenu;
        }
    }

    private void Update()
    {
        if (isFade) return;

        if (!isMenu)
        {
            if (Input.anyKeyDown)
            {
                ReturnToMenu(video);
            }
        }
        else
        {
            if (IdleCheck())
            {
                isFade = true;
                TransitionManager.Instance.NormalFadeIn(0.5f, () =>
                {
                    SceneManager.LoadScene("Trailer");
                });
            }

            if (Input.anyKey)
            {
                _lastIdleTime = Time.time;                
            }
        }
    }

    private bool IdleCheck()
    {
        return Time.time - _lastIdleTime > idleTimeSetting;
    }

    private void ReturnToMenu(VideoPlayer vp)
    {
        isFade = true;
        TransitionManager.Instance.SceneFadeIn(0.5f, () =>
        {
            SceneManager.LoadScene("Menu");
        });
    }
}
