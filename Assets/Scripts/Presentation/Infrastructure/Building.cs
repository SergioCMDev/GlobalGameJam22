using System;
using App.Events;
using Presentation.Interfaces;
using Presentation.UI.Menus;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public class Building : MonoBehaviour, IReceiveDamage, ILife
    {
        [SerializeField] private SliderBarView _sliderBarView;
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        [SerializeField] protected float _maximumLife, _alphaWhenSelected = 0.475f;
        [SerializeField] private Vector3Int _area;
        private int _id, _level;
        protected float _currentLife;
        private bool _placed;
        protected Color originalColor;
        protected Color colorWithTransparency;
        public event Action<Building> OnBuildingDestroyed;


        public float Life
        {
            get => _currentLife;
            private set => _currentLife = value;
        }

        public Vector3Int Area => _area;

        protected SpriteRenderer SpriteRenderer => _spriteRenderer;

        private void Awake()
        {
            var color = SpriteRenderer.color;
            originalColor = color;
            colorWithTransparency = new Color(color.r, color.g, color.b, _alphaWhenSelected);
        }

        private void UpdateLifeToMaximum()
        {
            _currentLife = _maximumLife;
            _sliderBarView.SetMaxValue(_maximumLife);
            UpdateLifeSliderBar();
        }

        public void Initialize()
        {
            Awake();
            UpdateLifeToMaximum();
        }

        public void ReceiveDamage(BuildingReceiveDamageEvent damageEvent)
        {
            ReceiveDamage(damageEvent.Damage);
        }
        
        private void DestroyBuilding()
        {
            OnBuildingDestroyed?.Invoke(this);
        }
        
        public void AddLife(BuildingReceiveLifeEvent receiveLifeEvent)
        {
            if (_id != receiveLifeEvent.Id) return;
            AddLife(receiveLifeEvent.Life);
        }

        private void UpdateLifeSliderBar()
        {
            _sliderBarView.SetValue(Life);
        }

        public void ReceiveDamage(float receivedDamage)
        {
            Life -= receivedDamage;
            UpdateLifeSliderBar();
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

            UpdateLifeSliderBar();
        }
    }
}