using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

public class MenuHudManager : MonoBehaviour
{
    public enum Page
    {
        TITLE,
        SAVE,
        OPTIONS,
        CREDIT
    }

    [SerializeField]
    private CanvasGroup[] _huds;
    private Page _currentPage = Page.TITLE;
    [SerializeField]
    private EventSystem _eventSystem;

    [Header("Title HUD")]
    [SerializeField]
    private Button _newGameButton;
    [SerializeField]
    private Button _continueButton;
    [SerializeField]
    private CanvasGroup _continueCanvasGroup;
    [SerializeField]
    private Button _optionsButton;
    [SerializeField]
    private Button _creditButton;
    [SerializeField]
    private Button _exitButton;
    [SerializeField]
    private TMP_Text _versionText;
    [SerializeField]
    private Animator _anim;
    
    [Header("Save HUD")]
    [SerializeField]
    private Button _backToTitleButton;
    [SerializeField]
    private Transform _saveRoot;

    [Header("Save Slot")]
    [SerializeField]
    private GameObject _saveSlot;
    [SerializeField]
    private TMP_Text _saveNameText;
    [SerializeField]
    private TMP_Text _playTimeText;
    [SerializeField]
    private TMP_Text _saveDateText;
    [SerializeField]
    private TMP_Text _saveTimeText;

    IEnumerator Start()
    {
        _versionText.text = $"v {Application.version}";

        _newGameButton.onClick.AddListener(NewGame);
        bool usedToPlay = SaveManager.Instance.GetSaveFiles().Count > 0;
        _continueButton.interactable = usedToPlay;
        _continueCanvasGroup.alpha = usedToPlay? 1 : 0.5f;
        _continueButton.onClick.AddListener(() => ChangePage(Page.SAVE));
        //_optionsButton.onClick.AddListener();
        //_creditButton.onClick.AddListener();
        _exitButton.onClick.AddListener(Exit);

        ChangePage(_currentPage);
        FetchSaveList();

        TransitionManager.Instance.SceneFadeOut();
        yield return new WaitForSeconds(6.4f);
        _anim.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (_currentPage)
            {
                case Page.TITLE:
                    break;
                case Page.SAVE:
                    ChangePage(Page.TITLE);
                    break;
                case Page.OPTIONS:
                    ChangePage(Page.TITLE);
                    break;
                case Page.CREDIT:
                    ChangePage(Page.TITLE);
                    break;
                default:
                    break;
            }
        }
    }

    private void ChangePage(Page page)
    {
        if (page == _currentPage) return;
        foreach (CanvasGroup canvas in _huds)
        {
            canvas.LeanAlpha(0, 0.3f);
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }

        _huds[(int)page].LeanAlpha(1, 0.3f).setDelay(0.3f);
        _huds[(int)page].interactable = true;
        _huds[(int)page].blocksRaycasts = true;

        switch (page)
        {
            case Page.TITLE:
                break;
            case Page.SAVE:
                _saveRoot.GetChild(0).gameObject.GetComponent<Button>().Select();
                break;
            case Page.OPTIONS:
                break;
            case Page.CREDIT:
                break;
            default:
                break;
        }

        _currentPage = page;
    }

    private void FetchSaveList()
    {
        var saveList = SaveManager.Instance.GetSaveFiles();

        for (int i = 0; i < 4; i++)
        {
            if (i <= saveList.Count - 1)
            {
                Player player = SaveManager.Instance.Load(int.Parse(saveList[i]));
                TimeSpan duration = player.lastDate - player.startDate;

                _saveNameText.text = i == 0? "AUTOSAVE" : "SLOT " + player.id;
                _playTimeText.text = _playTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", duration.Hours, duration.Minutes, duration.Seconds);
                _saveDateText.text = $"{player.lastDate.ToString("dd - MM - yyyy")}";
                _saveTimeText.text = $"{player.lastDate.ToString("hh:mm tt")}";

                GameObject GO = Instantiate(_saveSlot, _saveRoot);
                GO.GetComponent<Button>().onClick.AddListener(() => ContinueGame(player));
                GO.SetActive(true);
            }
            else
            {
                _saveNameText.text = i == 0 ? "AUTOSAVE" : "SLOT " + i;
                _playTimeText.text = "--.--.--";
                _saveDateText.text = "-- / -- / --";
                _saveTimeText.text = "--:--";

                GameObject GO = Instantiate(_saveSlot, _saveRoot);
                GO.SetActive(true);
            }
        }
    }

    private void NewGame()
    {
        Player player = new Player(0, false, false, false, false, false, false, false, false, false, DateTime.Now, DateTime.Now);

        SaveManager.Instance.Save(player);
        PlayerData.Instance.PlayerSetup(SaveManager.Instance.Load(player.id));
        TransitionManager.Instance.SceneFadeIn(0.5f, () =>
            SceneManager.LoadScene("Intro"));
    }

    private void ContinueGame(Player player)
    {
        PlayerData.Instance.PlayerSetup(player);
        StartGame();
    }

    private void StartGame()
    {
        TransitionManager.Instance.SceneFadeIn(0.5f, () =>
            SceneManager.LoadScene("Hikari"));
    }

    private void Exit()
    {
        TransitionManager.Instance.SceneFadeIn(0.5f, () => Application.Quit());
    }
}
