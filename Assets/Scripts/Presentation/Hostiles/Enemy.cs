using System;
using Presentation.Interfaces;
using Presentation.Menus;
using UnityEngine;
using Utils;

namespace Presentation.Hostiles
{
    public class Enemy : MonoBehaviour, IReceiveDamage, ILife, IStatusApplier
    {
        [SerializeField] private EnemyMovement enemyMovement;
        [SerializeField] private EnemyAttacker enemyAttacker;
        [SerializeField] private int _maximumLife;
        [SerializeField] private SliderBarView _sliderBarView;
        private float _life;
        private bool _isAlive;

        public event Action OnEnemyHasBeenDefeated;

        public EnemyMovement EnemyMovement => enemyMovement;
        public EnemyAttacker EnemyAttacker => enemyAttacker;

        private void Start()
        {
            _life = _maximumLife;
            _sliderBarView.SetMaxValue(_life);
        }

        private void UpdateLifeBar()
        {
            _sliderBarView.SetValue(_life);
        }

        public void ReceiveDamage(float receivedDamage)
        {
            _life -= receivedDamage;
            UpdateLifeBar();
            if (!IsAlive())
            {
                EnemyMovement.Stop();
                OnEnemyHasBeenDefeated.Invoke();
            }
        }

        public void ReduceSpeed(float percentageToReduce, float effectDuration)
        {
            Invoke(nameof(ResetSpeed), effectDuration);
            EnemyMovement.ChangeSpeed(EnemyMovement.Speed *= percentageToReduce);
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