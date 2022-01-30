using System;
using Presentation;

public class CityBuilding : Building, IDestructible
{
    public event Action<Building> OnBuildingDestroyed;


    public override void ReceiveDamage(float receivedDamage)
    {
        _currentLife -= receivedDamage;
        if ((((100/MaxLife)*Life)% 8) == 0)
        {
            DestroyBuilding();
        }
        else if (_currentLife <= 0)
        {
            DestroyBuilding();
        }
    }

    public void DestroyBuilding()
    {
        OnBuildingDestroyed.Invoke(this);
    }
}