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
        throw new System.NotImplementedException();
    }

    public void EnableInteract()
    {
        throw new System.NotImplementedException();
    }
}
