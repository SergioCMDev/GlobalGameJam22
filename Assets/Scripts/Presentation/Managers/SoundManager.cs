using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct MusicAudioClipLogic
{
    public AudioClip AudioClip;
    public MusicSoundName MusicSoundName;
}

[Serializable]
public struct SfxAudioClipLogic
{
    public AudioClip AudioClip;
    public SfxSoundName SfxSoundName;
}

[Serializable]
public enum SfxSoundName
{
    PlayerPickUpArrow,
    StepNormal1,
    StepNormal2,
    StepNormal3,
    StepNormal4,
    StepMush1,
    StepMush2,
    StepMush3,
    StepMush4,
    Dead1,
    Dead2,
    ThrowArrow,
    Fall,
    ClickUI,
    DoorOpening,
    PlatformDestruction,
    BallDestruction,
    PlayerPickUpCoin,
    FloorButtonActivation,
    BatMove,
    ArrowHit,
    Human,
    EnemyDead,
    SpiderEnemy
}


[Serializable]
public enum MusicSoundName
{
    MainMenu,
    BackgroundLevel1,
    BackgroundLevel2
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _sfx;
    [SerializeField] private AudioSource _music;
    [SerializeField] private List<SfxAudioClipLogic> _sfxSoundDictionary;
    [SerializeField] private List<MusicAudioClipLogic> _musicSoundDictionary;
    private bool _mutedByPlayer = false;

    private void Awake()
    {
        _music.enabled = !_mutedByPlayer;
    }

    //From Event
    public void PlaySFX(PlaySFXEvent playSfxEvent)
    {
        Debug.Log($"SFX {playSfxEvent.soundName}");
        PlaySfx(_sfxSoundDictionary.Single(x => x.SfxSoundName == playSfxEvent.soundName).AudioClip);
    }


    //From Event
    public void PlayMusic(PlayMusicEvent playMusicEvent)
    {
        PlayMusic(_musicSoundDictionary.Single(x => x.MusicSoundName == playMusicEvent.soundName).AudioClip);
    }


    private void PlaySfx(AudioClip audioClip)
    {
        _sfx.PlayOneShot(audioClip);
    }

    private void PlayMusic(AudioClip audioClip)
    {
        _music.loop = true;
        _music.clip = audioClip;
        _music.Play();
    }

    public void SetSfxVolume(float value)
    {
        _sfx.volume = Mathf.Clamp01(value);
    }

    public void SetMusicVolume(float value)
    {
        _music.volume = Mathf.Clamp01(value);
    }

    public float GetSfxVolume()
    {
        return _sfx.volume;
    }

    public float GetMusicVolume()
    {
        return _music.volume;
    }

    public void SetMusicEnabled(bool enabled)
    {
        _music.enabled = enabled;
    }

    public void SetSfxEnabled(bool enabled)
    {
        _sfx.enabled = enabled;
    }

    public void ToggleSound()
    {
        _sfx.mute = !_sfx.mute;
        _music.mute = !_music.mute;
        _mutedByPlayer = !_mutedByPlayer;
    }

    public bool IsMusicMuted()
    {
        return _mutedByPlayer;
    }

    public void LoadAudioData(AudioData audioData)
    {
        _sfx.volume = audioData.SFXVolume;
        _music.volume = audioData.MusicVolume;
        _mutedByPlayer = audioData.Muted;
        _sfx.mute = _mutedByPlayer;
        _music.mute = _mutedByPlayer;
    }

    public AudioData GetAudioData()
    {
        return new AudioData()
        {
            Muted = _mutedByPlayer,
            MusicVolume = _music.volume,
            SFXVolume = _sfx.volume
        };
    }
}