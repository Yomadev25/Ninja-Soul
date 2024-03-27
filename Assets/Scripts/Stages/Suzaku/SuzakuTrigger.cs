using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuzakuTrigger : MonoBehaviour,IInteract
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
        if (stageObj == null) return;
        stageObj.SetActive(SuzakuManager.Instance.stage == stage);
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
