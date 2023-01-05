using UnityEngine;

namespace App.Buildings
{
    [CreateAssetMenu(fileName = "ResourcesCostToUpgrade", menuName = "Resources/ResourcesCostToUpgrade")]
    public class BuildingCost : ScriptableObject
    {
        public int GoldCostToUpgrade;
        public int MetalCostToUpgrade;
        public MilitaryBuildingType militaryBuildingType;
    }
}