using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByakkoSwitch : MonoBehaviour, IInteract
{
    public const string MessageWantToActivateSwitch = "Want To Activate Switch";
    public int id;

    public void DisableInteract()
    {
        
    }

    public void EnableInteract()
    {
        
    }

    public void Interact()
    {
        MessagingCenter.Send(this, MessageWantToActivateSwitch);
        TransitionManager.Instance.SceneFadeIn(0.5f, () => UnityEngine.SceneManagement.SceneManager.LoadScene("Byakko_2"));
    }
}
