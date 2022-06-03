using System;
using System.Collections.Generic;
using App;
using Presentation.Infrastructure;
using Presentation.Interfaces;
using Presentation.Managers;
using Presentation.UI.Menus;
using UnityEngine;
using Utils;

namespace Presentation.Hostiles
{
    public class Enemy : MonoBehaviour, IReceiveDamage, ILife, IStatusApplier, IMovable
    {
        [SerializeField] private EnemyMovement enemyMovement;
        [SerializeField] private EnemyAttacker enemyAttacker;
        [SerializeField] private SliderBarView sliderBarView;
        private TilesToFollow tilesToFollow;

        private float _life;
        private float _maximumLife;
        private int _currentCityToDestroyed;
        private bool _isAlive;
        private TilePosition _nextDestination;
        private List<Building> _cityBuilding;
        private Building _cityTarget;
        private GridPathfinding _gridPathfinding;
        private Timer _timer;

        public event Action<Enemy> OnEnemyHasBeenDefeated;

        private EnemyMovement EnemyMovement => enemyMovement;

        // public List<TilePosition> TilesToFollow => tilesToFollow.TilePositions;


        public void Init(Vector3Int initialPosition, List<Building> cityBuilding, GridPathfinding pathfinding, EnemySpawnerInfo enemyInfo)
        {
            _maximumLife = enemyInfo.enemyInfo.life;
            enemyMovement.Speed = enemyInfo.enemyInfo.movementSpeed;
            _life = _maximumLife;
            sliderBarView.SetMaxValue(_maximumLife);
            _cityBuilding = cityBuilding;
            _currentCityToDestroyed = 0;
            enemyAttacker.Init(enemyInfo.enemyInfo);
            _cityTarget = cityBuilding[_currentCityToDestroyed];
            _gridPathfinding = pathfinding;
            enemyMovement.tilePosition = _gridPathfinding.InitialTile;
            _nextDestination = _gridPathfinding.GetNextPositionFromCurrent(enemyMovement.tilePosition);
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
            Deactivate();
            OnEnemyHasBeenDefeated.Invoke(this);
        }

        public void ReduceSpeed(float percentageToReduce, float effectDuration)
        {
            _timer = new Timer();
            _timer.Init(effectDuration);
            _timer.OnTimerEnds += ResetSpeed;
            StartCoroutine(_timer.CountTIme());
            
            EnemyMovement.ChangeSpeed(EnemyMovement.Speed * (1-percentageToReduce));
        }

        public bool IsAlive()
        {
            return _life > 0;
        }

        private void ResetSpeed()
        {
            _timer.OnTimerEnds -= ResetSpeed;

            EnemyMovement.ResetSpeed();
        }

        public void AddLife(float lifeToAdd)
        {
            _life = _life.CircularClamp(0, _maximumLife);
            UpdateLifeBar();
        }

        public void Deactivate()
        {
            _isAlive = false;
            EnemyMovement.Stop();
        }

        private bool HasToFindNewDestination()
        {
            return transform.position == _nextDestination.WorldPosition;
        }

        private void Update()
        {
            if (!_isAlive) return;
            //Ver donde se encuentra
            //COmprobar si esta en la ultima posicion
            //SI NO  => Obtener siguiente posicion con respecto a la actual
            //SI ESTA => ATACAR
            if (enemyMovement.tilePosition == _gridPathfinding.LastPosition)
            {
                if (!enemyAttacker.CanAttack() || _cityTarget == null) return;
                enemyAttacker.Attack(_cityTarget);
                if (_cityTarget.Life > 0) return;
                _currentCityToDestroyed++;
                if (ExistsMoreTargets())
                {
                    ChangeTarget();
                }

                return;
            }

            if (enemyMovement.transform.position ==
                _nextDestination.WorldPosition) //BUscar manera para saber si llego a dicha casilla
            {
                enemyMovement.tilePosition = _nextDestination;

                if (enemyMovement.tilePosition == _gridPathfinding.LastPosition)
                {
                    return;
                }

                _nextDestination = _gridPathfinding.GetNextPositionFromCurrent(enemyMovement.tilePosition);
                OnObjectMoved?.Invoke(gameObject, new WorldPositionTuple()
                {
                    NewWorldPosition = _nextDestination.WorldPosition,
                    OldWorldPosition = transform.position
                });
            }

            enemyMovement.MoveTo(_nextDestination.WorldPosition);
        }

        private bool ExistsMoreTargets()
        {
            return _currentCityToDestroyed <= _cityBuilding.Count;
        }

        private void ChangeTarget()
        {
            _cityTarget = _cityBuilding[_currentCityToDestroyed];
        }

        private void DestroyTile(Building obj)
        {
            // if (_defensiveBuilds.Any())
            // {
            //     //var randomKey = _defensiveBuilds.Keys.ToArray()[(int)Random.Range(0, _defensiveBuilds.Keys.Count - 1)];
            //     var randomKey = _defensiveBuilds[(int)Random.Range(0, _defensiveBuilds.Count - 1)];
            //     _tilemap.SetTile(randomKey.GridPosition, cityDestroyed);
            //     _defensiveBuilds.Remove(randomKey);
            // }
        }


        public event Action<GameObject, WorldPositionTuple> OnObjectMoved;
    }
}