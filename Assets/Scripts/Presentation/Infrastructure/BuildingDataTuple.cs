using System;
using App;
using UnityEngine;

namespace Presentation.Infrastructure
{
    [Serializable]
    public struct BuildingDataTuple
    {
        public BuildingType BuildingType;
        public GameObject Prefab;
    }
}