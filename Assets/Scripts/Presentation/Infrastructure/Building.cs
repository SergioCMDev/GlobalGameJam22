using App.Events;
using Presentation.Interfaces;
using Presentation.Structs;
using Presentation.UI.Menus;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public abstract class Building : MonoBehaviour, IReceiveDamage, ILife
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

        public Vector3Int Area => _area;

        protected SpriteRenderer SpriteRenderer => _spriteRenderer;

        private void Awake()
        {
            var color = SpriteRenderer.color;
            originalColor = color;
            colorWithTransparency = new Color(color.r, color.g, color.b,
                _alphaWhenSelected);
        }

        void Start()
        {
            _currentLife = _maximumLife;
            _sliderBarView.SetMaxValue(Life);
        }

        public void Initialize()
        {
            Awake();
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

        public void ReceiveDamage(float receivedDamage, DamageType damageType)
        {
            ReceiveDamage(receivedDamage);
        }
    }
}