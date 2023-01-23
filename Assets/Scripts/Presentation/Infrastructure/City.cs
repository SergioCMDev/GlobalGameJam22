using System;
using App.Events;
using Presentation.Interfaces;
using Presentation.UI.Menus;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public class City : Building, IReceiveDamage, ILife
    {
        private float _currentLife;
        [SerializeField] private float _maximumLife;
        [SerializeField] protected SliderBarView sliderBarView;
        public event Action<City> OnBuildingDestroyed;

        protected internal override void Initialize()
        {
            base.Initialize();
            UpdateLifeToMaximum(_maximumLife);
        }

        public void UpdateLifeToMaximum(float maxLife)
        {
            _currentLife = maxLife;
            sliderBarView.SetMaxValue(_currentLife);
        }

        public float Life
        {
            get => _currentLife;
            private set => _currentLife = value;
        }

        public void ReceiveDamage(float receivedDamage)
        {
            Life -= receivedDamage;
            sliderBarView.SetValue(_currentLife);

            if (Life <= 0)
            {
                DestroyBuilding();
            }
        }

        public bool IsAlive()
        {
            return Life > 0;
        }

        public void AddLife(float lifeToAdd)
        {
            Life += lifeToAdd;
            if (Life > _maximumLife)
            {
                Life = _maximumLife;
            }
        }

        private void DestroyBuilding()
        {
            OnBuildingDestroyed?.Invoke(this);
        }
    }
}