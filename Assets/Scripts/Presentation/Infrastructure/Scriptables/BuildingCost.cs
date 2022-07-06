using App;
using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    [CreateAssetMenu(fileName = "ResourcesCostToUpgrade", menuName = "Resources/ResourcesCostToUpgrade")]
    public class BuildingCost : ScriptableObject
    {
        public int GoldCostToUpgrade;
        public int MetalCostToUpgrade;
        public MiltaryBuildingType militaryBuildingType;
    }
}