using Application_.Events;
using Presentation.Menus;
using UnityEngine;

namespace Presentation
{
    public abstract class Building : MonoBehaviour, IReceiveDamage, ILife, IConstructable
    {
        [SerializeField] private SliderBarView _sliderBarView;

        [SerializeField] protected float  _maximumLife;
        private int _id, _level;
        protected float _currentLife;
        protected int Level
        {
            get => _level;
            set => _level = value;
        }

        public float Life
        {
            get => _currentLife;
            set => _currentLife = value;
        }
        
        public float MaxLife
        {
            get => _maximumLife;
            set => _maximumLife = value;
        }

        void Start()
        {
            _sliderBarView.SetMaxValue(Life);
        }

        public void ReceiveDamage(BuildingReceiveDamageEvent damageEvent)
        {
            ReceiveDamage(damageEvent.Damage);
        }

        public virtual void AddLife(BuildingReceiveLifeEvent receiveLifeEvent)
        {
            if (_id != receiveLifeEvent.Id) return;
            AddLife(receiveLifeEvent.Life);
        }

        protected void UpdateLifeSliderBar()
        {
            _sliderBarView.SetValue(Life);
        }

        public abstract void ReceiveDamage(float receivedDamage);

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

            UpdateLifeSliderBar();
        }

        public void ConstructBuilding()
        {
            throw new System.NotImplementedException();
        }

        public void ReceiveDamage(float receivedDamage, DamageType damageType)
        {
            ReceiveDamage(receivedDamage);
        }
    }
}