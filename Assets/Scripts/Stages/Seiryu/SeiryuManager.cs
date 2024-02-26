using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeiryuManager : Singleton<SeiryuManager>
{
    [SerializeField]
    private Transform[] _checkPoints;

    [Header("Boss")]
    [SerializeField]
    private GameObject _seiryuLeader;

    private bool stage1;
    private bool stage2;
    private bool stage3;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += SceneLoaded;

        MessagingCenter.Subscribe<SeiryuTrigger, int>(this, SeiryuTrigger.MessageTriggerStage, (sender, stage) =>
        {
            InitStage(stage);
        });
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        MessagingCenter.Unsubscribe<SeiryuTrigger, int>(this, SeiryuTrigger.MessageTriggerStage);
    }

    private void SceneLoaded(Scene s, LoadSceneMode e)
    {
        if (s.name == "HUD") return;

        if (s.name != "Seiryu_1")
        {
            Destroy(gameObject);
        }
    }

    public void InitStage(int stage)
    {
        switch (stage)
        {
            case 1:
                if (stage1 == true) return;
                stage1 = true;
                break;
            case 2:
                if (stage2 == true) return;
                stage2 = true;
                break;
            case 3:
                if (stage3 == true) return;
                SpawnBoss();
                stage3 = true;
                break;
            default:
                break;
        }
    }

    private void SpawnBoss()
    {
        _seiryuLeader.SetActive(true);
    }
}
