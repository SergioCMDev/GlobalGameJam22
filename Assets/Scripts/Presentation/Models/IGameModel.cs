internal interface IGameModel
{
    void PauseGame();
    void ContinueGame();
    void RestartScene();
    void QuitGame();
    void SaveGame(int currentIdSavePoint);
    Savegame LoadGame();
    bool HasSavedGame();
    void SetNewGameStatus(bool statusToSave);
    void DeleteLastSaveGame();
}