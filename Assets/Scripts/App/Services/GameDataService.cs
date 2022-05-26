using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Utils;

namespace App.Services
{
    public class GameDataService
    {
        private ILoader _loader;
        private ISaver _saver;

        public GameDataService()
        {
            _loader = ServiceLocator.Instance.GetService<ILoader>();
            _saver = ServiceLocator.Instance.GetService<ISaver>();
        }
        
        public bool HasStartedGame()
        {
           return _loader.HasSavedGame();
        }

        public void SaveGame(string lastCompletedScene)
        {
            var lastSavedGame = _loader.LoadGame();
            if (lastSavedGame == null)
            {
                lastSavedGame = new Savegame();
            }
            
            lastSavedGame.LastCompletedSceneName = lastCompletedScene;
            _saver.SaveGame(lastSavedGame);
        }
        
        public string GetLastLevelPlayed()
        {
           var lastPastLevel = _loader.LoadGame().LastCompletedSceneName;
           
            return lastPastLevel;
        }
    }
}