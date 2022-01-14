using System;
using System.Collections.Generic;

namespace Domain
{
    [Serializable]
    public class Savegame
    {
        public List<LevelData> SavePointIdOfLevel;
        public AudioData AudioData;
        public int CollectedCoins;
        public float MaximumLife;
        public float CurrentLife;
        public int MaximumArrows;

        public Savegame()
        {
            MaximumArrows = 6;
            SavePointIdOfLevel = new List<LevelData>();
            AudioData = new AudioData();
        }
    }

    [Serializable]
    public class LevelData
    {
        public int Level;
        public string LevelName;
        public int SavePointId;
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