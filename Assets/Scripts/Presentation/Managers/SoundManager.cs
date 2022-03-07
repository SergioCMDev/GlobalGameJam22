using System;
using System.Collections.Generic;
using System.Linq;
using App.Events;
using App.Services;
using UnityEngine;

namespace Presentation.Managers
{
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
        public void PlaySfx(PlaySFXEvent playSfxEvent)
        {
            PlaySfx(playSfxEvent.soundName);
        }
        
        public void PlaySfx(SfxSoundName sfxSoundName)
        {
            Debug.Log($"SFX {sfxSoundName}");
            PlaySfx(_sfxSoundDictionary.Single(x => x.SfxSoundName == sfxSoundName).AudioClip);
        }


        //From Event
        public void PlayMusic(PlayMusicEvent playMusicEvent)
        {
            PlayMusic(playMusicEvent.soundName);
        }
        
        public void PlayMusic(MusicSoundName musicSoundName)
        {
            PlayMusic(_musicSoundDictionary.Single(x => x.MusicSoundName == musicSoundName).AudioClip);
        }

        
        public void StopMusic()
        {
            _music.Stop();
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

        public void LoadAudioData(AudioDataInfo audioData)
        {
            _sfx.volume = audioData.SFXVolume;
            _music.volume = audioData.MusicVolume;
            _mutedByPlayer = audioData.Muted;
            _sfx.mute = _mutedByPlayer;
            _music.mute = _mutedByPlayer;
        }

        public AudioDataInfo GetAudioData()
        {
            return new AudioDataInfo()
            {
                Muted = _mutedByPlayer,
                MusicVolume = _music.volume,
                SFXVolume = _sfx.volume
            };
        }
    }
}