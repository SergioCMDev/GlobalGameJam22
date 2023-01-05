namespace Services.MilitaryBuilding
{
    public interface IMilitaryBuilding
    {
        void Init();
        void Deploy();
        void Deactivate();
        void ActivateBuilding();
        void CleanOccupiers();
    }
}