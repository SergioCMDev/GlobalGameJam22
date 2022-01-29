using System;
using Presentation;
using UnityEngine;

public class EnemyBuilding : Building, IDestructible
{
    public event Action<Building> OnBuildingDestroyed;
    public override void ReceiveDamage(GameObject itemWhichHit, float receivedDamage)
    {
        ReceiveDamage(receivedDamage);
    }

    public override void ReceiveDamage(float receivedDamage)
    {
        _life -= receivedDamage;
        if (_life <= 0)
        {
            DestroyBuilding();
        }
    }

    public void DestroyBuilding()
    {
        OnBuildingDestroyed.Invoke(this);
    }

}