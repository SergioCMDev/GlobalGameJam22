using Domain;
using Utils;

namespace Services.GameData
{
    public class GameDataService : LoadableComponent

    {
    private readonly ILoader _loader;
    private readonly ISaver _saver;

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
        throw new System.NotImplementedException();
    }
    }
}