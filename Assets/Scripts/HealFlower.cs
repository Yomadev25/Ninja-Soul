using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealFlower : MonoBehaviour, IInteract
{
    public const string MessageWantToRecoverPlayer = "Want To Recover Player";

    public void DisableInteract()
    {
        
    }

    public void EnableInteract()
    {
        
    }

    public void Interact()
    {
        MessagingCenter.Send(this, MessageWantToRecoverPlayer);
    }
}
