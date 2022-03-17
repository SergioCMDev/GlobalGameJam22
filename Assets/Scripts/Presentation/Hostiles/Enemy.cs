using System;
using Presentation.Building;
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
        [SerializeField] private int maximumLife;
        [SerializeField] private SliderBarView sliderBarView;
        
        private float _life;
        private bool _isAlive;
        private TileDataEntity nextDestination;
        private CityBuilding cityBuilding;
        private GridPathfinding gridPathfinding;

        public event Action<Enemy> OnEnemyHasBeenDefeated;

        public EnemyMovement EnemyMovement => enemyMovement;
        public EnemyAttacker EnemyAttacker => enemyAttacker;

        private void Start()
        {
            _isAlive = false;
            _life = maximumLife;
            sliderBarView.SetMaxValue(_life);
        }

        public void Init(Vector3Int initialPosition, CityBuilding cityBuilding, GridPathfinding pathfinding)
        {
            this.cityBuilding = cityBuilding;
            gridPathfinding = pathfinding;
            // cityBuilding.OnBuildingDestroyed += DestroyTile; //mover a enemy
            nextDestination = gridPathfinding.GetNextPositionFromCurrent(transform.position);
            _isAlive = true; 
        }

        private void UpdateLifeBar()
        {
            sliderBarView.SetValue(_life);
        }

        public void ReceiveDamage(float receivedDamage)
        {
            _life -= receivedDamage;
            UpdateLifeBar();
            if (IsAlive()) return;
            EnemyMovement.Stop();
            OnEnemyHasBeenDefeated.Invoke(this);
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
            _life = _life.CircularClamp(0, maximumLife);
            UpdateLifeBar();
        }


        private void Update()
        {
            if (!_isAlive) return;
            if (!HasToFindNewDestination())
            {
                //attacking = true;
                //nextDestination = lastBrick;
                if (enemyAttacker.CanAttack() && cityBuilding.Life >= 0)
                {
                    EnemyAttacker.Attack(cityBuilding);
                }
            }
            else
            {
                nextDestination = gridPathfinding.GetNextPositionFromCurrent(transform.position);
            }

            enemyMovement.MoveTo(nextDestination.WorldPosition);
        }

        private void DestroyTile(Building.Building obj)
        {
            // if (_defensiveBuilds.Any())
            // {
            //     //var randomKey = _defensiveBuilds.Keys.ToArray()[(int)Random.Range(0, _defensiveBuilds.Keys.Count - 1)];
            //     var randomKey = _defensiveBuilds[(int)Random.Range(0, _defensiveBuilds.Count - 1)];
            //     _tilemap.SetTile(randomKey.GridPosition, cityDestroyed);
            //     _defensiveBuilds.Remove(randomKey);
            // }
        }

        private bool HasToFindNewDestination()
        {
            return transform.position == nextDestination.WorldPosition;
        }
    }
}