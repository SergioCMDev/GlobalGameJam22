using Application_.Events;
using Presentation.Menus;
using UnityEngine;

namespace Presentation
{
    public abstract class Building : MonoBehaviour, IReceiveDamage, ILife, IConstructable, IDestructible
    {
        [SerializeField] private SliderBarView _sliderBarViewEnemy;
        private int _id, _level;

        [SerializeField] protected float _life;

        protected int Level
        {
            get => _level;
            set => _level = value;
        }
        protected float Life
        {
            get => _life;
            set => _life = value;
        }

        void Start()
        {
            _sliderBarViewEnemy.SetMaxValue(Life);
        }


        public void ReceiveDamage(BuildingReceiveDamageEvent damageEvent)
        {
            if (_id != damageEvent.Id) return;
            ReceiveDamage(damageEvent.Damage);
        }

        public void AddLife(BuildingReceiveLifeEvent receiveLifeEvent)
        {
            if (_id != receiveLifeEvent.Id) return;
            AddLife(receiveLifeEvent.Life);
        }

        protected void UpdateLifeSliderBar()
        {
            _sliderBarViewEnemy.SetValue(Life);
        }

        public abstract void ReceiveDamage(GameObject itemWhichHit, float receivedDamage);

        public abstract void ReceiveDamage(float receivedDamage);

        public abstract void AddLife(float lifeToAdd);
        public void ConstructBuilding()
        {
            throw new System.NotImplementedException();
        }

        public void DestroyBuilding()
        {
            throw new System.NotImplementedException();
        }
    }
}