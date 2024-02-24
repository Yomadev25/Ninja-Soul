using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Intro : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer video;
    [SerializeField]
    private CanvasGroup skipText;

    private bool readyToSkip;
    private bool isCompleting;
    private Coroutine readyCoroutine;

    private void Start()
    {
        TransitionManager.Instance.SceneFadeOut();

        video.loopPointReached += (x) =>
        {
            Complete();
        };
    }

    private void Update()
    {
        if (!readyToSkip)
        {
            if (Input.anyKeyDown)
            {
                if (readyCoroutine != null)
                {
                    StopCoroutine(readyCoroutine);
                }
                readyCoroutine = StartCoroutine(ReadyCountdown());
            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
                Complete();
            }
        }
    }

    IEnumerator ReadyCountdown()
    {
        readyToSkip = true;
        skipText.LeanAlpha(1f, 0.5f);

        yield return new WaitForSeconds(3f);

        readyToSkip = false;
        skipText.LeanAlpha(0f, 0.5f);
    }

    private void Complete()
    {
        if (isCompleting) return;
        isCompleting = true;
        TransitionManager.Instance.SceneFadeIn(1f, () =>
        {
            SceneManager.LoadScene("Hikari");
        });
    }
}
