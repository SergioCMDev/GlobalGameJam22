using System;
using Application_.Events;
using Presentation.Interfaces;
using UnityEngine;
using Utils;

namespace Presentation.Building
{
    public enum AttackRangeType
    {
        Ring,
        Square
    }

    public abstract class MilitaryBuilding : Building, IAttack
    {
        [SerializeField] private float _cadence, _damage, _distanceToAttack;
        [SerializeField] private int _attackRingRange = 1;
        [SerializeField] private SfxSoundName _sfxWhenAttack;
        [SerializeField] private DamageType _damageType;
        [SerializeField] private PlaySFXEvent _playSfxEvent;
        [SerializeField] private GameObject _particles;
        [SerializeField] private PlacerBuildingView _chooserCanvas;
        public event Action OnBuildingTriesToTakePlace, OnCancelTakingPlace;

        //TODO TO BASIC TURRET?
        [SerializeField] private Vector3Int _attackArea;
        //TODO TO BASIC TURRET?
        [SerializeField] private AttackRangeType _attackAreaType;
        private IReceiveDamage enemyToAttack;
        private GameObject enemyGameObject;

        private float _lastTimeAttacked;
        private bool _enemyIsSet;
        private bool _isActive;

        private float Cadence
        {
            get => _cadence;
            set => _cadence = value;
        }

        private float Damage
        {
            get => _damage;
            set => _damage = value;
        }

        public Vector3Int AttackArea => _attackArea;

        public AttackRangeType AttackAreaType => _attackAreaType;

        public int AttackRingRange => _attackRingRange;

        private protected float DistanceToAttack
        {
            get => _distanceToAttack;
        }


        private void Awake()
        {
            _chooserCanvas.gameObject.SetActive(false);
            _chooserCanvas.OnCancelTakingPlace += CancelTakingPlace;
            _chooserCanvas.OnBuildingTriesToTakePlace += BuildingTriesToTakePlace;
        }

        //TODO FIND WHERE TO PUT THIS, A NEW UPPER MANAGER? 
        private void CancelTakingPlace()
        {
            OnCancelTakingPlace();
        }

        private void BuildingTriesToTakePlace()
        {
            OnBuildingTriesToTakePlace();
        }

        public override void ReceiveDamage(float receivedDamage)
        {
            Life -= receivedDamage;
            UpdateLifeSliderBar();
        }

        public void Attack(IReceiveDamage objectToAttack)
        {
            _lastTimeAttacked = Time.deltaTime;

            PlaySoundWhenAttacks();
            ThrowParticlesWhenAttacks();
            objectToAttack.ReceiveDamage(Damage, _damageType);
        }

        protected abstract void ThrowParticlesWhenAttacks();

        private void PlaySoundWhenAttacks()
        {
            _playSfxEvent.soundName = _sfxWhenAttack;
            _playSfxEvent.Fire();
        }


        protected abstract bool CanReach(GameObject objectToAttack);

        private void Update()
        {
            if (!_isActive || !_enemyIsSet || !CanAttack() || !CanReach(enemyGameObject)) return;

            Debug.Log("ATTACK");
            Attack(enemyToAttack);
        }

        //TODO REFRACTOR
        private bool CanAttack()
        {
            _lastTimeAttacked += Time.deltaTime;
            return _lastTimeAttacked > Cadence;
        }

        public void SetEnemyToAttack(GameObject enemy)
        {
            if (enemy == null) return;
            enemyToAttack = enemy.GetComponent<IReceiveDamage>();
            enemyGameObject = enemy;
            _enemyIsSet = true;
            _isActive = true;
        }

        public void SetStatusChooserCanvas(bool status)
        {
            _chooserCanvas.gameObject.SetActive(status);
        }

        public void Select()
        {
            _spriteRenderer.color = colorWithTransparency;
        }

        public void Deselect()
        {
            _spriteRenderer.color = originalColor;
        }

        private void OnDrawGizmos()
        {
            if (!_enemyIsSet) return;
            Gizmos.color = Color.magenta;
            var directionToEnemy = enemyGameObject.transform.position - transform.position;
            Gizmos.DrawRay(transform.position, transform.TransformDirection(directionToEnemy) * DistanceToAttack);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.TransformDirection(directionToEnemy));
        }

        public void Stop()
        {
            _isActive = false;
        }
    }
}