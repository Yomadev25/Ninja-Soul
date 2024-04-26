using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class F_Cutscene2 : MonoBehaviour
{
    [SerializeField]
    private Dialogue _dialogue;

    private void Awake()
    {
        MessagingCenter.Subscribe<DialogueManager, Dialogue>(this, DialogueManager.MessageOnDialogueEnded, (sender, dialogue) =>
        {
            if (dialogue == _dialogue)
            {
                TransitionManager.Instance.NormalFadeIn(0.5f, () =>
                {
                    SceneManager.LoadScene("Final");
                });
            }
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<DialogueManager, Dialogue>(this, DialogueManager.MessageOnDialogueEnded);
    }

    private void Start()
    {
        DialogueManager.Instance.ActivateDialogue(_dialogue);
    }
}
