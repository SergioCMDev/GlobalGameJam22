using System;
using Application_;
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