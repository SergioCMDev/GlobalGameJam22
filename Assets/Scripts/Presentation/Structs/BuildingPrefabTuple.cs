using System;
using UnityEngine;

namespace Presentation.Structs
{
    [Serializable]
    public struct BuildingPrefabTuple
    {
        public BuildingSelector BuildingSelectable;
        public GameObject BuildingPrefab;
    }
}