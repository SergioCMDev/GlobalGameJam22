using System;
using Presentation.Infrastructure;

namespace Presentation.Interfaces
{
    public interface IDestructible
    {
        void DestroyBuilding();
        event Action<Building> OnBuildingDestroyed;
    }
}