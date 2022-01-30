using System;
using Presentation;
using UnityEngine;

public class CityBuilding : Building, IDestructible
{
    public event Action<Building> OnBuildingDestroyed;
    public override void ReceiveDamage(GameObject itemWhichHit, float receivedDamage)
    {
        ReceiveDamage(receivedDamage);
    }

    public override void ReceiveDamage(float receivedDamage)
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