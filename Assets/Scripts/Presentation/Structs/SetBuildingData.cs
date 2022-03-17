using System;
using Presentation.Infrastructure;
using UnityEngine;

namespace Presentation.Structs
{
    [Serializable]
    public struct SetBuildingData
    {
        public GameObject building;
        public MilitaryBuildingFacade buildingFacadeComponent;
        public Vector3Int position;
    }
}