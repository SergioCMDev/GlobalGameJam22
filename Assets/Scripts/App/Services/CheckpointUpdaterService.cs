using System.Linq;
using Domain;
using Utils;

namespace App.Services
{
    public class CheckpointUpdaterService
    {
        private Savegame _savegame;
        private ILoader _loader;
        private ISaver _saver;

        public CheckpointUpdaterService()
        {
            _loader = ServiceLocator.Instance.GetService<ILoader>();
            _saver = ServiceLocator.Instance.GetService<ISaver>();
        }

        public int GetLatestCheckpoint(int sceneBuildIndex)
        {
            var checkpointId = 0;
            if (!_loader.HasSavedGame()) return checkpointId;

            _savegame = _loader.LoadGame();
            var savedPointsOfLevel =
                _savegame.SavePointIdOfLevel.SingleOrDefault(x => x.Level == sceneBuildIndex);
            if (savedPointsOfLevel != null)
            {
                checkpointId = savedPointsOfLevel.SavePointId;
            }

            return checkpointId;
        }

        public void UpdateLastCheckpoint(int checkpointID, int sceneBuildIndex)
        {
            var levelBuildIndexId = sceneBuildIndex;
            var savegame = new Savegame();

            if (_loader.HasSavedGame())
            {
                savegame = _loader.LoadGame();
                var savedPointsOfLevel =
                    savegame.SavePointIdOfLevel.SingleOrDefault(x => x.Level == levelBuildIndexId);
                if (savedPointsOfLevel != null)
                {
                    savegame.SavePointIdOfLevel.Remove(savedPointsOfLevel);
                }
            }

            var levelInfos = new LevelData
            {
                Level = levelBuildIndexId,
                LevelName = SceneUtils.GetCurrentScene(),
                SavePointId = checkpointID
            };

            savegame.SavePointIdOfLevel.Add(levelInfos);

            _saver.SaveGame(savegame);
        }
    }
}