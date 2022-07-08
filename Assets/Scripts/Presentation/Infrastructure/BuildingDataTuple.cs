using System;
using App;
using UnityEngine;

namespace Presentation.Infrastructure
{
    [Serializable]
    public struct BuildingDataTuple
    {
        public MilitaryBuildingType militaryBuildingType;
        public GameObject Prefab;
    }
}