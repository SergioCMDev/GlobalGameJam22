using System;
using Presentation.Interfaces;

namespace Presentation.Building
{
    public class CityBuilding : Building, IDestructible
    {
        public event Action<Building> OnBuildingDestroyed;


        public void ReceiveDamage(float receivedDamage, int citySize)
        {
            ReceiveDamage(receivedDamage);
            if ((((100/MaxLife)*Life)% citySize) == 0)
            {
                DestroyBuilding();
            }
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
}