using System;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public class CityBuilding : Building
    {
        public event Action<Building> OnBuildingDestroyed;

        [SerializeField] private float citySize;

        // [SerializeField] private CityInfo cityInfo;
        public override void ReceiveDamage(float receivedDamage)
        {
            Life -= receivedDamage;
            //TODO Refactor, create a method to describe what is their meaning
            UpdateLifeSliderBar();
            Debug.Log("Damage percentage: " + (100 / MaxLife) * Life);
            //ChangeSprite Depending on their life
            // if (CurrentLife() == 0)
            if (Life <= 0)
            {
                DestroyBuilding();
            }
        }

        private float CurrentLife()
        {
            return 100 / MaxLife * Life % citySize;
            // if (_currentLife <= 0)
        }

        private void DestroyBuilding()
        {
            OnBuildingDestroyed?.Invoke(this);
        }
    }
}