using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SettingManager : Singleton<SettingManager>
{
    public enum DifficultLevel
    {
        EASY,
        MEDIUM,
        HARD,
        EXTRA
    }

    [Header("Gameplay Setting")]
    [SerializeField]
    private DifficultLevel _difficultLevel;

    [Header("Sound Setting")]
    [SerializeField]
    [Range(0f, 1f)]
    private float _masterVolume;
    [SerializeField]
    [Range(0f, 1f)]
    private float _musicVolume;
    [SerializeField]
    [Range(0f, 1f)]
    private float _soundEffectVolume;

    [Header("Graphic Setting")]
    [SerializeField]
    private RenderPipelineAsset[] _graphicQualities;
    [SerializeField]
    private QualityLevel _graphicQuality;

    [Header("Control Setting")]
    [SerializeField]
    private Key _forwardKey;

    public void SetGraphicQuality(int level)
    {
        _graphicQuality = (QualityLevel)level;

        QualitySettings.SetQualityLevel((int)_graphicQuality);
        QualitySettings.renderPipeline = _graphicQualities[(int)_graphicQuality];
    }
}
