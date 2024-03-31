using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SettingManager : Singleton<SettingManager>
{
    public enum GraphicQuality
    {
        Low,
        Medium,
        High
    }

    [Header("Sound Setting")]
    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField]
    [Range(0f, 1f)]
    private float _masterVolume;
    [SerializeField]
    [Range(0f, 1f)]
    private float _musicVolume;
    [SerializeField]
    [Range(0f, 1f)]
    private float _soundEffectVolume;
    public float masterVolume => _masterVolume;
    public float musicVolume => _musicVolume;
    public float soundEffectVolume => _soundEffectVolume;


    [Header("Graphic Setting")]
    [SerializeField]
    private int _fullscreen;
    [SerializeField]
    private GraphicQuality _graphicLevel;
    [SerializeField]
    private int _resolution;
    public Resolution[] resolutions;
    public int fullscreen => _fullscreen;
    public GraphicQuality graphicLevel => _graphicLevel;
    public int resolution => _resolution;

    protected override void Awake()
    {
        base.Awake();

        MessagingCenter.Subscribe<OptionHudManager, float>(this, OptionHudManager.MessageOnMasterVolumeChanged, (sender, value) =>
        {
            AdjustMasterVolume(value);
        });

        MessagingCenter.Subscribe<OptionHudManager, float>(this, OptionHudManager.MessageOnMusicVolumeChanged, (sender, value) =>
        {
            AdjustMusicVolume(value);
        });

        MessagingCenter.Subscribe<OptionHudManager, float>(this, OptionHudManager.MessageOnSoundEffectVolumeChanged, (sender, value) =>
        {
            AdjustSoundEffectVolume(value);
        });

        MessagingCenter.Subscribe<OptionHudManager, int>(this, OptionHudManager.MessageOnGraphicLevelChanged, (sender, value) =>
        {
            SetGraphic(value);
        });

        MessagingCenter.Subscribe<OptionHudManager, int>(this, OptionHudManager.MessageOnWindowModeChanged, (sender, value) =>
        {
            SetFullscreen(value);
        });

        MessagingCenter.Subscribe<OptionHudManager, int>(this, OptionHudManager.MessageOnResolutionChanged, (sender, value) =>
        {
            SetResolution(value);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<OptionHudManager, float>(this, OptionHudManager.MessageOnMasterVolumeChanged);
        MessagingCenter.Unsubscribe<OptionHudManager, float>(this, OptionHudManager.MessageOnMusicVolumeChanged);
        MessagingCenter.Unsubscribe<OptionHudManager, float>(this, OptionHudManager.MessageOnSoundEffectVolumeChanged);
        MessagingCenter.Unsubscribe<OptionHudManager, int>(this, OptionHudManager.MessageOnGraphicLevelChanged);
        MessagingCenter.Unsubscribe<OptionHudManager, int>(this, OptionHudManager.MessageOnWindowModeChanged);
        MessagingCenter.Unsubscribe<OptionHudManager, int>(this, OptionHudManager.MessageOnResolutionChanged);
    }

    private void Start()
    {
        resolutions = Screen.resolutions;

        _fullscreen = PlayerPrefs.GetInt("Fullscreen", 1);
        SetFullscreen(_fullscreen);
        _graphicLevel = (GraphicQuality)PlayerPrefs.GetInt("Graphic", 2);
        SetGraphic((int)_graphicLevel);
        _resolution = PlayerPrefs.GetInt("Resolution", resolutions.Length - 1);
        SetResolution(_resolution);

        LoadAudioVolume();
    }

    private void LoadAudioVolume()
    {
        AdjustMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1f));
        AdjustMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1f));
        AdjustSoundEffectVolume(PlayerPrefs.GetFloat("SFXVolume", 1f));
    }

    private void AdjustMasterVolume(float value)
    {
        if (value == 0) value = 0.001f;

        _audioMixer.SetFloat("Master", Mathf.Log10(value) * 20);
        _masterVolume = value;

        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    private void AdjustMusicVolume(float value)
    {
        if (value == 0) value = 0.001f;

        _audioMixer.SetFloat("Music", Mathf.Log10(value) * 20);
        _musicVolume = value;

        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private void AdjustSoundEffectVolume(float value)
    {
        if (value == 0) value = 0.001f;

        _audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
        _soundEffectVolume = value;

        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private void SetFullscreen(int value)
    {
        Screen.fullScreen = value == 1;
        PlayerPrefs.SetFloat("Fullscreen", value);
    }

    private void SetGraphic(int value)
    {
        QualitySettings.SetQualityLevel(value);
        _graphicLevel = (GraphicQuality)value;

        PlayerPrefs.SetFloat("Graphic", value);
    }

    private void SetResolution(int value)
    {
        Resolution res = resolutions[value];
        Screen.SetResolution(res.width, res.height, _fullscreen == 1);
        _resolution = value;

        PlayerPrefs.SetFloat("Resolution", value);
    }
}
