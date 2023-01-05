using Domain;
using UnityEngine;
using Utils;

namespace Services.SoundSystem
{
    [CreateAssetMenu(fileName = "SoundDataService",
        menuName = "Loadable/Services/SoundDataService")]
    public class SoundDataService : LoadableComponent
    {
        private ILoader _loader;
        private ISaver _saver;

        public SoundDataService()
        {
            _loader = ServiceLocator.Instance.GetService<ILoader>();
            _saver = ServiceLocator.Instance.GetService<ISaver>();
        }

        public AudioDataInfo LoadAudioData()
        {
            if (!_loader.HasSavedGame()) return new AudioDataInfo();

            var savegame = _loader.LoadGame();
            var audioData = savegame.AudioData;
            var audioDataInfo = TransformToAudioDataInfo(audioData);
            return audioDataInfo;
        }

        private AudioDataInfo TransformToAudioDataInfo(AudioData audioData)
        {
            return new AudioDataInfo()
            {
                MusicVolume = audioData.MusicVolume,
                SFXVolume = audioData.SFXVolume,
                Muted = audioData.Muted
            };
        }

        public void SaveAudioData(AudioDataInfo audioData)
        {
            var savegame = new Savegame();

            if (_loader.HasSavedGame())
            {
                savegame = _loader.LoadGame();
            }

            savegame.AudioData.Muted =
                audioData.Muted != savegame.AudioData.Muted ? audioData.Muted : savegame.AudioData.Muted;
            savegame.AudioData.SFXVolume = audioData.SFXVolume != savegame.AudioData.SFXVolume
                ? audioData.SFXVolume
                : savegame.AudioData.SFXVolume;
            savegame.AudioData.MusicVolume = audioData.MusicVolume != savegame.AudioData.MusicVolume
                ? audioData.MusicVolume
                : savegame.AudioData.MusicVolume;

            _saver.SaveGame(savegame);
        }

        public override void Execute()
        {
        }
    }
}