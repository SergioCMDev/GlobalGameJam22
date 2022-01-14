public interface ISaver
{
    void SaveGame(Savegame savegameFile);
    void DeleteSaveGame();
    void SaveNewGameStatus(bool statusToSave);
}