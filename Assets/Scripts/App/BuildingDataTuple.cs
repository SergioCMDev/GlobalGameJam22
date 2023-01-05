using System;
using UnityEngine;

namespace App
{
    [Serializable]
    public struct BuildingDataTuple
    {
        public MilitaryBuildingType militaryBuildingType;
        public GameObject Prefab;
    }
}