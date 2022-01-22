using Application_.Events;
using Presentation.UI;
using UnityEngine;

namespace Presentation
{
    public class Building : MonoBehaviour, IReceiveDamage, ILife
    {
        [SerializeField] private SliderBarView _sliderBarViewEnemy;
        private int _id;

        [SerializeField] private float _life;

        void Start()
        {
            _sliderBarViewEnemy.SetMaxValue(_life);
        }

        public void ReceiveDamage(BuildingReceiveDamageEvent damageEvent)
        {
            if(_id != damageEvent.Id) return;
            ReceiveDamage(damageEvent.Damage);
        }
        
        public void AddLife(BuildingReceiveLifeEvent receiveLifeEvent)
        {
            if(_id != receiveLifeEvent.Id) return;
            AddLife(receiveLifeEvent.Life);
        }


        public void ReceiveDamage(GameObject itemWhichHit, float receivedDamage)
        {
        }

        public void ReceiveDamage(float receivedDamage)
        {
            _life -= receivedDamage;
            _sliderBarViewEnemy.SetValue(_life);
        }

        public void AddLife(float lifeToAdd)
        {
            _life += lifeToAdd;
            _sliderBarViewEnemy.SetValue(_life);
        }
    }
}