using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField]
    private Button _hikariButton;
    [SerializeField]
    private Button _genbuButton;
    [SerializeField]
    private Button _seiryuButton;
    [SerializeField]
    private Button _suzakuButton;
    [SerializeField]
    private Button _byakkoButton;

    private void Start()
    {
        TransitionManager.Instance.SceneFadeOut();

        _hikariButton.onClick.AddListener(() => Play("Hikari"));
        _genbuButton.onClick.AddListener(() => Play("Genbu_1"));
        _seiryuButton.onClick.AddListener(() => Play("Seiryu_1"));
        _suzakuButton.onClick.AddListener(() => Play("Suzaku_1"));
        _byakkoButton.onClick.AddListener(() => Play("Byakko_1"));

        Player data = PlayerData.Instance.GetPlayerData();
        if (data.genbu)
        {
            _genbuButton.interactable = false;
            foreach (Transform item in _genbuButton.transform)
            {
                item.gameObject.SetActive(false);
            }
        }
        if (data.seiryu)
        {
            _seiryuButton.interactable = false;
            foreach (Transform item in _seiryuButton.transform)
            {
                item.gameObject.SetActive(false);
            }
        }
        if (data.suzaku)
        {
            _suzakuButton.interactable = false;
            foreach (Transform item in _suzakuButton.transform)
            {
                item.gameObject.SetActive(false);
            }
        }
        if (data.byakko)
        {
            _byakkoButton.interactable = false;
            foreach (Transform item in _byakkoButton.transform)
            {
                item.gameObject.SetActive(false);
            }
        }

        StageManager.Instance.ResetCreteria();        
    }

    private void Play(string name)
    {
        TransitionManager.Instance.SceneFadeIn(0.5f, () =>
        {
            SceneManager.LoadScene(name);
        });
    }
}
