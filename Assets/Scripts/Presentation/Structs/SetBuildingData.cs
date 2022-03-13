using System;
using UnityEngine;

namespace Presentation
{
    [Serializable]
    public struct SetBuildingData
    {
        public GameObject building;
        public MilitaryBuildingFacade buildingFacadeComponent;
        public Vector3Int position;
    }
}