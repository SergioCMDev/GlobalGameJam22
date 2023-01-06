using Domain;
using UnityEngine;
using Utils;

namespace Services.GameData
{
    [CreateAssetMenu(fileName = "GameDataService",
        menuName = "Loadable/Services/GameDataService")]
    public class GameDataService : LoadableComponent
    {
        private ILoader _loader;
        private ISaver _saver;

        public GameDataService()
        {

        }

        public bool HasStartedGame()
        {
            return _loader.HasSavedGame();
        }

        public void SaveGame(string lastCompletedScene)
        {
            var lastSavedGame = _loader.LoadGame() ?? new Savegame();

            lastSavedGame.NameOfLastCompletedScene = lastCompletedScene;
            var id = Utilities.GetNumberOfLevelString(lastCompletedScene);

            lastSavedGame.IdOfLastCompletedScene = id;
            _saver.SaveGame(lastSavedGame);
        }

        public int GetIdOfLastLevelPlayed()
        {
            var lastPastLevel = _loader.LoadGame().IdOfLastCompletedScene;

            return lastPastLevel;
        }

        public string GetNameOfLastLevelPlayed()
        {
            var lastPastLevel = _loader.LoadGame().NameOfLastCompletedScene;

            return lastPastLevel;
        }

        public override void Execute()
        {
            _loader = ServiceLocator.Instance.GetService<ILoader>();
            _saver = ServiceLocator.Instance.GetService<ISaver>();
            // throw new System.NotImplementedException();
        }
    }
}