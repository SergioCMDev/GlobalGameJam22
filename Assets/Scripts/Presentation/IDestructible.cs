using System;

namespace Presentation
{
    public interface IDestructible
    {
        void DestroyBuilding();
        event Action<Building> OnBuildingDestroyed;
    }
}