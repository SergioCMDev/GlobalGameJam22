using UnityEngine;
using Utils;

namespace Domain
{
    public class SaveUsingPlayerPrefs : ISaver
    {
        private IJsonator _jsonator;

        public SaveUsingPlayerPrefs()
        {
            _jsonator = ServiceLocator.Instance.GetService<IJsonator>();
        }

        public void SaveGame(Savegame savegameFile)
        {
            string dataToSaveJson = _jsonator.ToJson(savegameFile);
            PlayerPrefs.SetString("SaveGame", dataToSaveJson);
            PlayerPrefs.SetInt("HasSavedGame", 1);
            PlayerPrefs.Save();
        }

        public void DeleteSaveGame()
        {
            PlayerPrefs.DeleteKey("SaveGame");
            PlayerPrefs.Save();
        }

        public void SaveNewGameStatus(bool statusToSave)
        {
            PlayerPrefs.SetInt("HasSavedGame", statusToSave ? 1 : 0);
        }
    }
}