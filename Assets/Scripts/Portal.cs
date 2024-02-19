using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IInteract
{
    [SerializeField]
    private string _scene;

    public void Interact()
    {
        TransitionManager.Instance.SceneFadeIn(1f, () =>
        {
            SceneManager.LoadScene(_scene);
        });
    }

    public void DisableInteract()
    {
        
    }

    public void EnableInteract()
    {
        
    }
}
