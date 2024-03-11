using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeiryuManager : Singleton<SeiryuManager>
{
    [SerializeField]
    private StageCriteria _stageCriteria;
    [SerializeField]
    private StageIntro _stageIntro;

    public int stage;
    [SerializeField]
    private Transform[] _checkPoints;

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

    private void Start()
    {
        _stageIntro.gameObject.SetActive(true);
        _stageIntro.PlayIntro();
    }

    private void InitStage(int stage)
    {
        StageManager.Instance.InitCriteria(_stageCriteria);
        PlayerData.Instance.SetSpawnPoint(_checkPoints[(int)stage].position);
        this.stage = stage;

        var stageTriggers = FindObjectsOfType<SeiryuTrigger>();
        foreach (SeiryuTrigger trigger in stageTriggers)
        {
            trigger.UpdateStage();
        }
    }
}
