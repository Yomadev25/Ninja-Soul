using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    public const string MessageShowStageCriteria = "Show Stage Criteria";
    public const string MessageShowPlayTime = "Show Play Time";

    [SerializeField]
    private StageCriteria _stageCriteria;
    private StageCriteria _currentProgress = new StageCriteria();
    private StageCriteria _finalProgress = new StageCriteria();

    private DateTime _startTime;

    protected override void Awake()
    {
        base.Awake();
        MessagingCenter.Subscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyDead, (sender) =>
        {
            EliminateEnemy();
        });

        MessagingCenter.Subscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied, (sender) =>
        {
            PlayerDied();
        });

        MessagingCenter.Subscribe<GameManager>(this, GameManager.MessageOnLevelCompleted, (sender) =>
        {
            StageClear();
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyDead);
        MessagingCenter.Unsubscribe<PlayerManager>(this, PlayerManager.MessageOnPlayerDied);
        MessagingCenter.Unsubscribe<GameManager>(this, GameManager.MessageOnLevelCompleted);
    }

    private void Start()
    {
        ResetCreteria();
    }

    public void InitCriteria(StageCriteria criteria)
    {
        _stageCriteria = criteria;
    }

    public void ResetCreteria()
    {
        _stageCriteria = null;
        _currentProgress.deathCount = 0;
        _currentProgress.enemyCount = 0;
        _currentProgress.time = 0;

        _startTime = DateTime.Now;
    }

    private void EliminateEnemy()
    {
        _currentProgress.enemyCount++;
    }

    private void PlayerDied()
    {
        _currentProgress.deathCount++;
    }

    private void StageClear()
    {
        TimeSpan totalTime = DateTime.Now - _startTime;
        _currentProgress.time = (int)(totalTime.TotalMinutes);

        _finalProgress.enemyCount = _stageCriteria.enemyCount / _currentProgress.enemyCount;
        _finalProgress.deathCount = _stageCriteria.deathCount / (_stageCriteria.deathCount - _currentProgress.deathCount);
        _finalProgress.time = _stageCriteria.time / (_stageCriteria.time - _currentProgress.time);

        MessagingCenter.Send(this, MessageShowStageCriteria, _finalProgress);
        MessagingCenter.Send(this, MessageShowPlayTime, totalTime);
    }
}

[System.Serializable]
public class StageCriteria
{
    public int enemyCount;
    public int deathCount;
    public int time;
}
