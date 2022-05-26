using Domain;

namespace App.Models
{
    public interface IPlayerModel
    {
        void Init(Savegame savegame);
        void Init();
        void ResetData();
    }
}