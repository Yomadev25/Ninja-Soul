using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionHudManager : MonoBehaviour
{
    public const string MessageOnMasterVolumeChanged = "On Master Volume Changed";
    public const string MessageOnMusicVolumeChanged = "On Music Volume Changed";
    public const string MessageOnSoundEffectVolumeChanged = "On Sound Effect Volume Changed";

    public const string MessageOnGraphicLevelChanged = "On Graphic Level Changed";
    public const string MessageOnWindowModeChanged = "On Window Mode Change";
    public const string MessageOnResolutionChanged = "On Resolution Changed";

    [Header("Sound Options")]
    [SerializeField]
    private int _masterVolume;
    [SerializeField]
    private OptionItemHud _masterVolumeOption;
    [SerializeField]
    private int _musicVolume;
    [SerializeField]
    private OptionItemHud _musicVolumeOption;
    [SerializeField]
    private int _sfxVolume;
    [SerializeField]
    private OptionItemHud _sfxVolumeOption;

    [Header("Graphic Options")]
    [SerializeField]
    private int _graphicLevel;
    [SerializeField]
    private OptionItemHud _graphicLevelOption;
    [SerializeField]
    private int _windowMode;
    [SerializeField]
    private OptionItemHud _windowModeOption;
    [SerializeField]
    private int _resolution;
    [SerializeField]
    private OptionItemHud _resolutionOption;
    [SerializeField]
    private Resolution[] _resolutions;

    private void Awake()
    {
        MessagingCenter.Subscribe<OptionItemHud, bool>(this, OptionItemHud.MessageWantToAdjustOption, (sender, isNext) =>
        {
            if (sender == _masterVolumeOption)
            {
                _masterVolume += isNext ? 1 : -1;
                if (_masterVolume > 5) _masterVolume = 5;
                else if (_masterVolume < 0) _masterVolume = 0;

                _masterVolumeOption.UpdateOptionValue(_masterVolume);
                MessagingCenter.Send(this, MessageOnMasterVolumeChanged, (float)(_masterVolume / 5f));
            }
            else if (sender == _musicVolumeOption)
            {
                _musicVolume += isNext ? 1 : -1;
                if (_musicVolume > 5) _musicVolume = 5;
                else if (_musicVolume < 0) _musicVolume = 0;

                _musicVolumeOption.UpdateOptionValue(_musicVolume);
                MessagingCenter.Send(this, MessageOnMusicVolumeChanged, (float)(_musicVolume / 5f));
            }
            else if (sender == _sfxVolumeOption)
            {
                _sfxVolume += isNext ? 1 : -1;
                if (_sfxVolume > 5) _sfxVolume = 5;
                else if (_sfxVolume < 0) _sfxVolume = 0;

                _sfxVolumeOption.UpdateOptionValue(_sfxVolume);
                MessagingCenter.Send(this, MessageOnSoundEffectVolumeChanged, (float)(_sfxVolume / 5f));
            }
            else if (sender == _graphicLevelOption)
            {
                _graphicLevel += isNext ? 1 : -1;
                if (_graphicLevel > 2) _graphicLevel = 0;
                else if (_graphicLevel < 0) _graphicLevel = 2;

                _graphicLevelOption.UpdateOptionValue(((SettingManager.GraphicQuality)_graphicLevel).ToString());
                MessagingCenter.Send(this, MessageOnGraphicLevelChanged, _graphicLevel);
            }
            else if (sender == _windowModeOption)
            {
                _windowMode += isNext ? 1 : -1;
                if (_windowMode > 1) _windowMode = 0;
                else if (_windowMode < 0) _windowMode = 1;

                string mode = _windowMode == 1 ? "Fullscreen" : "Windowed";
                _windowModeOption.UpdateOptionValue(mode);
                MessagingCenter.Send(this, MessageOnWindowModeChanged, _windowMode);
            }
            else if (sender == _resolutionOption)
            {
                _resolution += isNext ? 1 : -1;
                if (_resolution > _resolutions.Length - 1)
                    _resolution = 0;
                else if (_resolution < 0) 
                    _resolution = _resolutions.Length - 1;

                RenderResolutionText();
                MessagingCenter.Send(this, MessageOnResolutionChanged, _resolution);
            }
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<OptionItemHud, bool>(this, OptionItemHud.MessageWantToAdjustOption);
    }

    private void Start()
    {
        InitSoundHud();
        InitGraphicHud();
    }

    private void InitSoundHud()
    {
        _masterVolume = (int)(SettingManager.Instance.masterVolume * 5);
        _musicVolume = (int)(SettingManager.Instance.musicVolume * 5);
        _sfxVolume = (int)(SettingManager.Instance.soundEffectVolume * 5);

        _masterVolumeOption.UpdateOptionValue(_masterVolume);
        _musicVolumeOption.UpdateOptionValue(_musicVolume);
        _sfxVolumeOption.UpdateOptionValue(_sfxVolume);
    }

    private void InitGraphicHud()
    {
        _graphicLevel = (int)SettingManager.Instance.graphicLevel;
        _windowMode = SettingManager.Instance.fullscreen;
        _resolution = SettingManager.Instance.resolution;
        _resolutions = SettingManager.Instance.resolutions;

        _graphicLevelOption.UpdateOptionValue(((SettingManager.GraphicQuality)_graphicLevel).ToString());
        string mode = _windowMode == 1 ? "Fullscreen" : "Windowed";
        _windowModeOption.UpdateOptionValue(mode);
        RenderResolutionText();
    }

    private void RenderResolutionText()
    {
        Resolution res = _resolutions[_resolution];
        string option = res.width + " x " + res.height;

        _resolutionOption.UpdateOptionValue(option);
    }
}
