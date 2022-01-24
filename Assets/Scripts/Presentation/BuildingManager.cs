using Application_;
using Application_.Models;
using UnityEngine;
using Utils;

namespace Presentation
{
    public class BuildingManager : MonoBehaviour
    {
        private IBuildingStatusModel _buildingStatusModel;

        // Start is called before the first frame update
        void Start()
        {
            _buildingStatusModel = ServiceLocator.Instance.GetModel<IBuildingStatusModel>();
            _buildingStatusModel.AddBuilding(new BuildStatus()
            {
                buildingType = BuildingType.Tesla,
                MaxLife = 50,
            });
        }
    }
}