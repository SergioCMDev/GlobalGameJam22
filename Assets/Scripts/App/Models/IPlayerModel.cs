using Domain;

namespace App.Models
{
    public interface IPlayerModel
    {
        void Init(Savegame savegame);
        void Init();
        void ResetData();
        float CurrentLife { get; set; }
        float MaxLife { get; }
        int CurrentArrows { get; set; }
        int MaximumArrows { get; set; }
        int CurrentCoins { get; set; }
        void IncreaseMaximumArrows();
    }
}