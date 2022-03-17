using System;
using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public class CityBuilding : Building, IDestructible
    {
        public event Action<Building> OnBuildingDestroyed;


        public void ReceiveDamage(float receivedDamage, int citySize)
        {
            ReceiveDamage(receivedDamage);
            //TODO Refactor, create a method to describe what is their meaning
            if (100 / MaxLife * Life % citySize == 0)
            {
                DestroyBuilding();
            }
        }

        public override void ReceiveDamage(float receivedDamage)
        {
            Debug.Log("Damage percentage: " + (100 / MaxLife) * Life);

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
}