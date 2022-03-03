namespace App.Services
{
    public class AudioDataInfo
    {
        public float MusicVolume;
        public float SFXVolume;
        public bool Muted;

        public AudioDataInfo()
        {
            MusicVolume = 1;
            SFXVolume = 1;
            Muted = false;
        }
    }
}