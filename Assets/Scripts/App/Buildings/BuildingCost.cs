using UnityEngine;

namespace App
{
    [CreateAssetMenu(fileName = "ResourcesCostToUpgrade", menuName = "Resources/ResourcesCostToUpgrade")]
    public class BuildingCost : ScriptableObject
    {
        public int GoldCostToUpgrade;
        public int MetalCostToUpgrade;
        public MilitaryBuildingType militaryBuildingType;
    }
}