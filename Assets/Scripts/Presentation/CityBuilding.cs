using System;
using Presentation;

public class CityBuilding : Building, IDestructible
{
    public event Action<Building> OnBuildingDestroyed;


    protected override void ReceiveDamage(float receivedDamage)
    {
        _currentLife -= receivedDamage;
        if (_currentLife <= 0)
        {
            DestroyBuilding();
        }
    }

    public void DestroyBuilding()
    {
        OnBuildingDestroyed.Invoke(this);
    }
}