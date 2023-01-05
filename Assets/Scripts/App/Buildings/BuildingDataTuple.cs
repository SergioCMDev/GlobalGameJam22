using System;
using UnityEngine;

namespace App.Buildings
{
    [Serializable]
    public struct BuildingDataTuple
    {
        public MilitaryBuildingType militaryBuildingType;
        public GameObject Prefab;
    }
}