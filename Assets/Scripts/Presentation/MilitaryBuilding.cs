using System;
using Application_.Events;
using UnityEngine;
using Utils;

namespace Presentation
{
    public abstract class MilitaryBuilding : Building, IAttack
    {
        [SerializeField] private float _cadence, _damage, _distanceToAttack;
        [SerializeField] private SfxSoundName _sfxWhenAttack;
        [SerializeField] private DamageType _damageType;
        [SerializeField] private PlaySFXEvent _playSfxEvent;
        [SerializeField] private GameObject _particles;
        [SerializeField] private PlacerBuildingView _chooserCanvas;
        public event Action OnBuildingTriesToTakePlace, OnCancelTakingPlace;

        private IReceiveDamage enemyToAttack;
        private ILife enemyLife;
        private GameObject enemyGameObject;

        private float _lastTimeAttacked;
        private bool _enemyIsSet;

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
            if (!Utilities.HasPastTime(_lastTimeAttacked, Cadence) || !CanReach(enemyGameObject)) return;
            _lastTimeAttacked = Time.deltaTime;
            MakeSoundWhenAttacks();
            ThrowParticlesWhenAttacks();
            objectToAttack.ReceiveDamage(Damage, _damageType);
        }

        protected abstract void ThrowParticlesWhenAttacks();

        private void MakeSoundWhenAttacks()
        {
            _playSfxEvent.soundName = _sfxWhenAttack;
            _playSfxEvent.Fire();
        }

        private bool CanReach(GameObject objectToAttack)
        {
            return VectorUtils.VectorIsNearVector(gameObject.transform.position, objectToAttack.transform.position,
                _distanceToAttack);
        }

        protected abstract bool CanReach();


        private void Update()
        {
            if (_enemyIsSet && enemyLife.IsAlive() && CanAttack() && CanReach(enemyGameObject))
            {
                Attack(enemyToAttack);
            }
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
            enemyLife = enemy.GetComponent<ILife>();
            enemyGameObject = enemy;
            _enemyIsSet = true;
        }

        public void SetStatusChooserCanvas(bool status)
        {
            _chooserCanvas.gameObject.SetActive(status);
        }
    }
}