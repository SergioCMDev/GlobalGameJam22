﻿namespace App.Buildings
{
    public class BuildStatus
    {
        public float CurrentLife;
        public float MaxLife;
        public MilitaryBuildingType MilitaryBuildingType;
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