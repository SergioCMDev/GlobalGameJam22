using System;
using UnityEngine;

namespace Presentation.Managers
{
    [Serializable]
    public struct LevelViewInfo
    {
        public Sprite levelImage;
        public string sceneToLoad;
        public int levelId;
    }
}