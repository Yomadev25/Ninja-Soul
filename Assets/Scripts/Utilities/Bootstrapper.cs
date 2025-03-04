using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Systems/Transition Manager")));
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Systems/Player Data")));
    }
}
