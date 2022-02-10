using System;

namespace Presentation.Interfaces
{
    public interface IDestructible
    {
        void DestroyBuilding();
        event Action<Building.Building> OnBuildingDestroyed;
    }
}