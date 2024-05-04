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

    private string _currentBgm;
    private string _currentOverrideBgm;
    public string currentBgm => _currentBgm;

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
                    LeanTween.value(0f, bgm.volume, 1f).setOnUpdate(x =>
                    {
                        _bgmSource.volume = x;
                    });
                });
            }
            else
            {
                _bgmSource.clip = bgm.clip;
                _bgmSource.volume = bgm.volume;
                _bgmSource.Play();
            }

            _currentBgm = name;
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

        _currentBgm = null;
        _currentOverrideBgm = null;
    }

    public void PlayOverrideBGM(string name, bool instant = false)
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
                    LeanTween.value(0f, bgm.volume, 1f).setOnUpdate(x =>
                    {
                        _bgmSource.volume = x;
                    });
                });
            }
            else
            {
                _bgmSource.clip = bgm.clip;
                _bgmSource.volume = bgm.volume;
                _bgmSource.Play();
            }
            _currentOverrideBgm = name;
        }
        else
        {
            Debug.LogWarning("Can't find " + name + " in bgm list");
        }
    }

    public void StopOverrideBGM(bool instant = false)
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
                PlayBGM(_currentBgm);
            });
        }
        else
        {
            _bgmSource.Stop();
            PlayBGM(_currentBgm);
        }

        _currentOverrideBgm = null;
    }

    public void PlaySFX(string name, float volume = 1f)
    {
        Audio sfx = Array.Find(_sfx, x => x.name == name);

        if (sfx != null)
        {
            if (sfx.clip == null)
            {
                Debug.LogWarning(sfx.name + " hasn't audio clip");
                return;
            }

            _sfxSource.volume = volume;
            _sfxSource.PlayOneShot(sfx.clip);
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
    [Range(0f, 1f)]
    public float volume = 1f;
}
