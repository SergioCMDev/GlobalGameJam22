namespace Application_
{
    public class BuildStatus
    {
        public float CurrentLife;
        public float MaxLife;
        public BuildingType buildingType;
        public float level;

        public BuildStatus()
        {
            level = 0;
        }
        
        public void Upgrade()
        {
            level++;
            CurrentLife = MaxLife;
        }
    }
}