using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioSource _bgmSource;
    [SerializeField]
    private Audio[] _bgm;

    [SerializeField]
    private AudioSource _sfxSource;
    [SerializeField]
    private Audio[] _sfx;

    public void PlayBGM(string name, bool instant = false)
    {
        Audio bgm = Array.Find(_bgm, x => x.name == name);

        if (bgm != null)
        {       
            if (bgm.clip == null)
            {
                Debug.LogWarning(bgm.name + " hasn't audio clip");
                return;
            }

            if (!instant)
            {
                float currentVolume = _bgmSource.volume;
                LeanTween.value(currentVolume, 0f, 1f).setOnUpdate(x =>
                {
                    _bgmSource.volume = x;
                }).setOnComplete(() =>
                {
                    _bgmSource.clip = bgm.clip;
                    _bgmSource.Play();
                    LeanTween.value(0f, 1f, 1f).setOnUpdate(x =>
                    {
                        _bgmSource.volume = x;
                    });
                });
            }
            else
            {
                _bgmSource.clip = bgm.clip;
                _bgmSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("Can't find " + name + " in bgm list");
        }
    }

    public void StopBGM(bool instant = false)
    {
        if (!instant)
        {
            float currentVolume = _bgmSource.volume;
            LeanTween.value(currentVolume, 0f, 1f).setOnUpdate(x =>
            {
                _bgmSource.volume = x;
            }).setOnComplete(() =>
            {
                _bgmSource.Stop();
            });
        }
        else
        {
            _bgmSource.Stop();
        }
    }

    public void PlaySFX(string name)
    {
        Audio sfx = Array.Find(_sfx, x => x.name == name);

        if (sfx != null)
        {
            if (sfx.clip == null)
            {
                Debug.LogWarning(sfx.name + " hasn't audio clip");
                return;
            }

            _sfxSource.clip = sfx.clip;
            _sfxSource.Play();
        }
        else
        {
            Debug.LogWarning("Can't find " + name + " in bgm list");
        }
    }
}

[Serializable]
public class Audio
{
    public string name;
    public AudioClip clip;
}
