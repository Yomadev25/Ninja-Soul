using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private ControllerTutorial _controllerTutorial;
    [SerializeField]
    private CombatTutorial _combatTutorial;
    [SerializeField]
    private SoulTutorial _soulTutorial;

    private void Awake()
    {
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
            _soulTutorial.gameObject.SetActive(false);
            GameManager.instance.LevelComplete();
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
        _controllerTutorial.gameObject.SetActive(true);
    }

    private void ActivateCombatTutorial()
    {
        _controllerTutorial.gameObject.SetActive(false);
        _combatTutorial.gameObject.SetActive(true);
    }

    private void ActivateSoulTutorial()
    {
        _combatTutorial.gameObject.SetActive(false);
        _soulTutorial.gameObject.SetActive(true);
    }
}
