using UnityEngine;
using UnityEngine.SceneManagement;

public class HudLoader : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }
}
