using Application_.Events;
using Presentation.Menus;
using UnityEngine;

namespace Presentation
{
    public abstract class Building : MonoBehaviour, IReceiveDamage, ILife
    {
        [SerializeField] private SliderBarView _sliderBarViewEnemy;
        private int _id;

        [SerializeField] protected float _life;

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
    }
}