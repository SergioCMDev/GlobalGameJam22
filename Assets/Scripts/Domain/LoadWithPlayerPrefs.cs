using App;
using UnityEngine;
using Utils;

namespace Domain
{
    public class LoadWithPlayerPrefs : ILoader
    {
        private IJsonator _jsonator;

        public LoadWithPlayerPrefs()
        {
            _jsonator = ServiceLocator.Instance.GetService<IJsonator>();
        }

        public Savegame LoadGame()
        {
            if (PlayerPrefs.HasKey("SaveGame"))
            {
                string dataToFromJson = PlayerPrefs.GetString("SaveGame");
                Savegame savegameJson = _jsonator.FromJson<Savegame>(dataToFromJson);
                return savegameJson;
            }

            return null;
        }

        public bool HasSavedGame()
        {
            return PlayerPrefs.GetInt("HasSavedGame") == 1;
        }
    }
}