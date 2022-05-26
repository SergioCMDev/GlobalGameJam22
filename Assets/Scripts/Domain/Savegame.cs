using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace App
{
    [Serializable]
    public class Savegame
    {
        public string NameOfLastCompletedScene;
        public int IdOfLastCompletedScene;
        public AudioData AudioData;

        public Savegame()
        {
            AudioData = new AudioData();
        }

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