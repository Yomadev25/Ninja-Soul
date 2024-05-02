using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalManager : Singleton<FinalManager>
{
    [SerializeField]
    private StageCriteria _stageCriteria;

    [Header("Soul Spawner")]
    [SerializeField]
    private float _minSpawnDuration;
    [SerializeField]
    private float _maxSpawnDuration;
    private float _duration;

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
        AudioManager.Instance.PlayBGM("Final Boss");
        if (s.name != "Final")
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StageManager.Instance.InitCriteria(_stageCriteria);
        TransitionManager.Instance.NormalFadeOut();
    }

    private void Update()
    {
        if (_duration > 0)
        {
            _duration -= Time.deltaTime;
        }
        else
        {
            if (SceneManager.GetActiveScene().name != "Final") return;

            _duration = Random.Range(_minSpawnDuration, _maxSpawnDuration);
            Vector3 position = Vector3.zero;
            position.x = Random.Range(-24f, -19f);
            position.y = 0.1f;
            position.z = Random.Range(-12f, 12f);
            EffectManager.Instance.Spawn("Soul", position, Quaternion.identity);
        }
    }
}
