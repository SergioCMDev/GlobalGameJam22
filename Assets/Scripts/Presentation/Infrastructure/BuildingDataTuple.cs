using System;
using App;
using UnityEngine;

namespace Presentation.Infrastructure
{
    [Serializable]
    public struct BuildingDataTuple
    {
        public MiltaryBuildingType miltaryBuildingType;
        public GameObject Prefab;
    }
}