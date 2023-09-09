using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionHudManager : MonoBehaviour
{
    public const string MessageOnMasterVolumeChanged = "On Master Volume Changed";
    public const string MessageOnMusicVolumeChanged = "On Music Volume Changed";
    public const string MessageOnSoundEffectVolumeChanged = "On Sound Effect Volume Changed";

    [Header("Sound Setting")]
    [SerializeField]
    private Slider _masterVolumeSlider;
    [SerializeField]
    private Slider _musicVolumeSlider;
    [SerializeField]
    private Slider _soundEffectVolumeSlider;

    private void Start()
    {
        InitSoundHud();       
    }

    private void InitSoundHud()
    {
        if (_masterVolumeSlider != null)
        {
            _masterVolumeSlider.onValueChanged.AddListener((value) =>
            {
                MessagingCenter.Send(this, MessageOnMasterVolumeChanged, value);
            });
        }
        else Debug.LogErrorFormat("{0} doesn't has {1}", nameof(Slider), this.name);

        if (_musicVolumeSlider != null)
        {
            _musicVolumeSlider.onValueChanged.AddListener((value) =>
            {
                MessagingCenter.Send(this, MessageOnMusicVolumeChanged, value);
            });
        }
        else Debug.LogErrorFormat("{0} doesn't has {1}", nameof(Slider), this.name);

        if (_soundEffectVolumeSlider != null)
        {
            _soundEffectVolumeSlider.onValueChanged.AddListener((value) =>
            {
                MessagingCenter.Send(this, MessageOnSoundEffectVolumeChanged, value);
            });
        }
        else Debug.LogErrorFormat("{0} doesn't has {1}", nameof(Slider), this.name);
    }
}
