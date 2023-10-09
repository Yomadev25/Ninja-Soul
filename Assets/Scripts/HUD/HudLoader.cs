using UnityEngine;
using UnityEngine.SceneManagement;

public class HudLoader : MonoBehaviour
{
    public const string MessageOnHudLoaded = "Loaded HUD";

    void Awake()
    {
        SceneManager.sceneLoaded += (s, e) =>
        {
            if (s.name != "HUD") return;
            MessagingCenter.Send(this, MessageOnHudLoaded);
        };

        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }
}
