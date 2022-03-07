﻿using System;
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

        public bool HasStartedGameWithCheckpoints()
        {
            if (!_loader.HasSavedGame()) return false;
            return _loader.LoadGame().SavePointIdOfLevel.Count > 1;
        }
        
        public bool HasStartedGame()
        {
           return _loader.HasSavedGame();
        }

        //TODO
        public void SaveGame(Savegame savegame)
        {
            _saver.SaveGame(savegame);
        }

        public int GetLastLevelPlayed()
        {
            List<LevelData> savePoints = _loader.LoadGame().SavePointIdOfLevel;
            List<int> valuesToCheck = new List<int>();
            foreach (var levelsChecked in savePoints)
            {
                valuesToCheck.Add(levelsChecked.Level);
            }

            int maximumLevel = Utilities.GetMaximumValueInList(valuesToCheck);
            return maximumLevel;
        }

        public string GetLastLevelNamePlayed()
        {
            List<LevelData> savePoints = _loader.LoadGame().SavePointIdOfLevel;
            List<int> valuesToCheck = new List<int>();
            foreach (var levelsChecked in savePoints)
            {
                var levelIdName = levelsChecked.LevelName.Substring(levelsChecked.LevelName.Length - 1, 1);
                valuesToCheck.Add(Int32.Parse(levelIdName));
            }

            int maximumLevel = Utilities.GetMaximumValueInList(valuesToCheck);
            var maximumLevelName = savePoints.Single(x => x.Level == maximumLevel).LevelName;
            return maximumLevelName;
        }
    }
}