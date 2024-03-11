using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public const string MessageOnTutorialComplete = "On Tutorial Complete";

    [SerializeField]
    private StageCriteria _stageCriteria;
    [SerializeField]
    private Dialogue[] _dialogues;
    [SerializeField]
    private ControllerTutorial _controllerTutorial;
    [SerializeField]
    private CombatTutorial _combatTutorial;
    [SerializeField]
    private SoulTutorial _soulTutorial;

    private void Awake()
    {
        MessagingCenter.Subscribe<DialogueManager, Dialogue>(this, DialogueManager.MessageOnDialogueEnded, (sender, dialogue) =>
        {
            if (dialogue == _dialogues[0])
            {
                _controllerTutorial.gameObject.SetActive(true);
                StageManager.Instance.ResetCreteria();
                StageManager.Instance.InitCriteria(_stageCriteria);
            }
            else if (dialogue == _dialogues[1])
            {
                _controllerTutorial.gameObject.SetActive(false);
                _combatTutorial.gameObject.SetActive(true);
            }
            else if (dialogue == _dialogues[2])
            {
                _combatTutorial.gameObject.SetActive(false);
                _soulTutorial.gameObject.SetActive(true);
            }
            else if (dialogue == _dialogues[3])
            {
                MessagingCenter.Send(this, MessageOnTutorialComplete);
            }
        });

        MessagingCenter.Subscribe<ControllerTutorial>(this, ControllerTutorial.MessageOnTutorialComplete, (sender) =>
        {
            Invoke(nameof(ActivateCombatTutorial), 3f);
        });

        MessagingCenter.Subscribe<CombatTutorial>(this, CombatTutorial.MessageOnTutorialComplete, (sender) =>
        {
            Invoke(nameof(ActivateSoulTutorial), 3f);
        });

        MessagingCenter.Subscribe<SoulTutorial>(this, SoulTutorial.MessageOnTutorialComplete, (sender) =>
        {
            DialogueManager.Instance.ActivateDialogue(_dialogues[3]);
            _soulTutorial.gameObject.SetActive(false);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<ControllerTutorial>(this, ControllerTutorial.MessageOnTutorialComplete);
        MessagingCenter.Unsubscribe<CombatTutorial>(this, CombatTutorial.MessageOnTutorialComplete);
        MessagingCenter.Unsubscribe<SoulTutorial>(this, SoulTutorial.MessageOnTutorialComplete);
    }

    private void Start()
    {
        DialogueManager.Instance.ActivateDialogue(_dialogues[0]);
    }

    private void ActivateCombatTutorial()
    {
        DialogueManager.Instance.ActivateDialogue(_dialogues[1]);
    }

    private void ActivateSoulTutorial()
    {
        DialogueManager.Instance.ActivateDialogue(_dialogues[2]);      
    }
}
