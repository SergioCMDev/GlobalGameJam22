using System;
using System.Collections.Generic;

namespace App
{
    [Serializable]
    public class Savegame
    {
        public List<LevelData> SavePointIdOfLevel;
        public AudioData AudioData;

        public Savegame()
        {
            SavePointIdOfLevel = new List<LevelData>();
            AudioData = new AudioData();
        }
    }

    [Serializable]
    public class LevelData
    {
        public int Level;
        public string LevelName;
    }
    
    [Serializable]
    public class AudioData
    {
        public float MusicVolume;
        public float SFXVolume;
        public bool Muted;

        public AudioData()
        {
            MusicVolume = 1;
            SFXVolume = 1;
            Muted = false;
        }
    }
}