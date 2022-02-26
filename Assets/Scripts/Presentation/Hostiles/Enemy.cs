using System;
using Presentation.Interfaces;
using Presentation.Menus;
using UnityEngine;
using Utils;

namespace Presentation.Hostiles
{
    public class Enemy : MonoBehaviour, IReceiveDamage, ILife
    {
        [SerializeField] private EnemyMovement enemyMovement;
        [SerializeField] private int _maximumLife;
        [SerializeField] private SliderBarView _sliderBarView;
        private float _life;
        private bool _isAlive;

        public event Action OnEnemyHasBeenDefeated;

        public EnemyMovement EnemyMovement => enemyMovement;

        private void Start()
        {
            _life = _maximumLife;
            _sliderBarView.SetMaxValue(_life);
        }

        private void UpdateLifeBar()
        {
            _sliderBarView.SetValue(_life);
        }

        public void ReceiveDamage(float receivedDamage, DamageType damageType)
        {
            //TODO REFRACTOR USING COMMAND PATTERN
            _life -= receivedDamage;
            UpdateLifeBar();
            if (!IsAlive())
            {
                EnemyMovement.Stop();
                OnEnemyHasBeenDefeated.Invoke();
            }
            
            if (damageType != DamageType.TeslaTower) return;
            EnemyMovement.ChangeSpeed(EnemyMovement.Speed *= 0.25f);
            Invoke(nameof(ResetSpeed), 0.4f);
        }

        public bool IsAlive()
        {
            return _life > 0;
        }

        private void ResetSpeed()
        {
            EnemyMovement.ResetSpeed();
        }

        public void AddLife(float lifeToAdd)
        {
            _life = _life.CircularClamp(0, _maximumLife);
            UpdateLifeBar();
        }
    }
}