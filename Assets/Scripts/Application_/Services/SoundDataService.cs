using Domain;
using Utils;

namespace Application_.Services
{
    public class SoundDataService
    {
        private ILoader _loader;
        private ISaver _saver;

        public SoundDataService()
        {
            _loader = ServiceLocator.Instance.GetService<ILoader>();
            _saver = ServiceLocator.Instance.GetService<ISaver>();
        }

        public AudioData LoadAudioData()
        {
            if (!_loader.HasSavedGame()) return new AudioData();

            var savegame = _loader.LoadGame();
            return savegame.AudioData;
        }

        public void SaveAudioData(AudioData audioData)
        {
            var savegame = new Savegame();
        
            if (_loader.HasSavedGame())
            {
                savegame = _loader.LoadGame();
            }

            savegame.AudioData.Muted = audioData.Muted != savegame.AudioData.Muted ? audioData.Muted : savegame.AudioData.Muted;
            savegame.AudioData.SFXVolume = audioData.SFXVolume != savegame.AudioData.SFXVolume ? audioData.SFXVolume : savegame.AudioData.SFXVolume;
            savegame.AudioData.MusicVolume = audioData.MusicVolume != savegame.AudioData.MusicVolume ? audioData.MusicVolume : savegame.AudioData.MusicVolume;
        
            _saver.SaveGame(savegame);
        }
    }
}