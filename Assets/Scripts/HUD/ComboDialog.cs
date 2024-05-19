using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboDialog : MonoBehaviour
{
    [SerializeField]
    private float _comboDuration;

    [Header("HUD")]
    [SerializeField]
    private CanvasGroup _comboHud;
    [SerializeField]
    private TMP_Text _comboText;

    private int _currentCombo;
    private float _currentDuration;

    private void Awake()
    {
        MessagingCenter.Subscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyTakeDamage, (sender) =>
        {
            AddCombo();
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyTakeDamage);
    }

    private void Update()
    {
        if (_currentCombo != 0)
        {
            if (_currentDuration > 0)
            {
                _currentDuration -= Time.deltaTime;
            }
            else
            {
                _currentCombo = 0;
                _comboHud.alpha = 0;
            }
        }
    }

    private void AddCombo()
    {
        _currentCombo++;
        _currentDuration = _comboDuration;
        _comboText.gameObject.SetActive(false);
        _comboText.text = _currentCombo.ToString();

        if (LeanTween.isTweening(_comboHud.gameObject))
        {
            LeanTween.cancel(_comboHud.gameObject);
        }

        _comboHud.LeanAlpha(1, 0.1f).setOnComplete(() =>
        {
            _comboHud.LeanAlpha(0, 3f).setDelay(4.9f);
        });
        _comboText.gameObject.SetActive(true);
    }
}
