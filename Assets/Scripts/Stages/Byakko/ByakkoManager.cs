using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ByakkoManager : Singleton<ByakkoManager> 
{ 
    [SerializeField]
    private StageCriteria _stageCriteria;
    [SerializeField]
    private StageIntro _stageIntro;

    [Header("Byakko Lobby")]
    public bool leftClear;
    public bool rightClear;

    [SerializeField]
    private List<int> _cacheInstanceID = new List<int>();


    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += SceneLoaded;

        MessagingCenter.Subscribe<ByakkoSwitch>(this, ByakkoSwitch.MessageWantToActivateSwitch, (sender) =>
        {
            if (sender.id == 1) leftClear = true;
            else if (sender.id == 2) rightClear = true;
        });
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoaded;

        MessagingCenter.Unsubscribe<ByakkoSwitch>(this, ByakkoSwitch.MessageWantToActivateSwitch);
    }

    private void SceneLoaded(Scene s, LoadSceneMode e)
    {
        if (s.name == "HUD") return;
        if (!s.name.Contains("Byakko"))
        {
            Destroy(gameObject);
        }
        else
        {
            var cacheObjects = FindObjectsOfType<CacheObject>();
            foreach (CacheObject cacheObject in cacheObjects)
            {
                if (!_cacheInstanceID.Contains(cacheObject.id))
                {
                    _cacheInstanceID.Add(cacheObject.id);
                }
            }
        }
    }

    private void Start()
    {
        StageManager.Instance.InitCriteria(_stageCriteria);
        Invoke(nameof(InitStage), 12f);

        _stageIntro.gameObject.SetActive(true);
        _stageIntro.PlayIntro();
    }

    private void InitStage()
    {

    }
}
