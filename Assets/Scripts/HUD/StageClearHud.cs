using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageClearHud : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _stageText;
    [SerializeField]
    private Image _defeatEnemyFill;
    [SerializeField]
    private Image _deathAvoidingFill;
    [SerializeField]
    private Image _playTimeFill;
    [SerializeField]
    private TMP_Text _playTimeText;
    [SerializeField]
    private TMP_Text _rankText;

    private void Awake()
    {
        MessagingCenter.Subscribe<StageManager, StageCriteria>(this, StageManager.MessageShowStageCriteria, (sender, criteria) =>
        {
            _stageText.text = criteria.stageName + " cleared";
            _defeatEnemyFill.fillAmount = criteria.enemyCount;
            _deathAvoidingFill.fillAmount = criteria.deathCount;
            _playTimeFill.fillAmount = criteria.time;

            criteria.enemyCount = Mathf.Clamp01(criteria.enemyCount);
            criteria.deathCount = Mathf.Clamp01(criteria.deathCount);
            criteria.time = Mathf.Clamp01(criteria.time);

            float average = (criteria.enemyCount + criteria.deathCount + criteria.time) / 3f;
            if (average >= 0.8f)
            {
                _rankText.text = "A";
            }
            else if (average >= 0.7f)
            {
                _rankText.text = "B";
            }
            else if (average >= 0.6f)
            {
                _rankText.text = "C";
            }
            else if (average >= 0.5f)
            {
                _rankText.text = "D";
            }
            else
            {
                _rankText.text = "E";
            }
        });

        MessagingCenter.Subscribe<StageManager, System.TimeSpan>(this, StageManager.MessageShowPlayTime, (sender, time) =>
        {
            _playTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<StageManager, StageCriteria>(this, StageManager.MessageShowStageCriteria);
        MessagingCenter.Unsubscribe<StageManager, System.TimeSpan>(this, StageManager.MessageShowPlayTime);
    }
}
