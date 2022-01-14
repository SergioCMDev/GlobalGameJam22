public class PlayerModel : IPlayerModel
{
    public float MaxLife { get; private set; }
    public float CurrentLife { get; set; }
    public int MaximumArrows { get; set; }
    public int CurrentCoins { get; set; }
    public int CurrentArrows { get; set; }

    public PlayerModel()
    {
        Init();
    }

    public void Init(Savegame savegame)
    {
        MaxLife = savegame.MaximumLife;
        MaximumArrows = savegame.MaximumArrows;
        CurrentLife = savegame.CurrentLife;
        CurrentCoins = savegame.CollectedCoins;
        SetArrows();
    }
    
    public void Init()
    {
        MaxLife = 100;
        MaximumArrows = 6;
        CurrentCoins = 0;
        SetLife();
        SetArrows();
    }
    
    private void SetArrows()
    {
        CurrentArrows = MaximumArrows;
    }

    private void SetLife()
    {
        CurrentLife = MaxLife;
    }
    

    public void ResetData()
    {
        SetLife();
        SetArrows();
    }
    
    public void IncreaseMaximumArrows()
    {
        MaximumArrows++;
        SetArrows();
    }
    
}