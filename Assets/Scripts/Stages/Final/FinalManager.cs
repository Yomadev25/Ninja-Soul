using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalManager : Singleton<FinalManager>
{
    [SerializeField]
    private StageCriteria _stageCriteria;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene s, LoadSceneMode e)
    {
        if (s.name == "HUD") return;
        if (s.name != "Final")
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StageManager.Instance.InitCriteria(_stageCriteria);
    }
}
