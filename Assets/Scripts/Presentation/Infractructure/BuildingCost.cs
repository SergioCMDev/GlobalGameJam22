using App;
using UnityEngine;

namespace Presentation.Building
{
    [CreateAssetMenu(fileName = "ResourcesCostToUpgrade", menuName = "Resources/ResourcesCostToUpgrade")]
    public class BuildingCost : ScriptableObject
    {
        public float GoldCostToUpgrade;
        public float MetalCostToUpgrade;
        public BuildingType BuildingType;
    }
}