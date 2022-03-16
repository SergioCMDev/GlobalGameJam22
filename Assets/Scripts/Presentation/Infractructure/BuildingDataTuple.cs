using System;
using App;
using UnityEngine;

namespace Presentation.Building
{
    [Serializable]
    public struct BuildingDataTuple
    {
        public BuildingType BuildingType;
        public GameObject Prefab;
    }
}