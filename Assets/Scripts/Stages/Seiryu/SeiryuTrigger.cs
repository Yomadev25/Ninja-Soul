using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeiryuTrigger : MonoBehaviour, IInteract
{
    public const string MessageTriggerStage = "Trigger Stage";
    public int stage;
    public GameObject stageObj;

    private void Start()
    {
        UpdateStage();
    }

    public void UpdateStage()
    {
        stageObj.SetActive(SeiryuManager.Instance.stage == stage);
    }

    public void DisableInteract()
    {
        
    }

    public void EnableInteract()
    {
        
    }

    public void Interact()
    {
        MessagingCenter.Send(this, MessageTriggerStage, stage);
    }
}
