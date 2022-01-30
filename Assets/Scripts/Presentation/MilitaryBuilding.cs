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

        private IReceiveDamage enemyToAttack;
        private GameObject enemy;

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

        public override void ReceiveDamage(float receivedDamage)
        {
            Life -= receivedDamage;
            UpdateLifeSliderBar();
        }

        public void Attack(IReceiveDamage objectToAttack)
        {
            if (!CanAttack() || !CanReach(enemy)) return;
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
            if (_enemyIsSet && CanAttack() && CanReach(enemy))
            {
                Attack(enemyToAttack);
            }
        }

        private bool CanAttack()
        {
            return _lastTimeAttacked + Cadence > Time.deltaTime;
        }

        public void SetEnemyToAttack(IReceiveDamage enemy)
        {
            enemyToAttack = enemy;
            _enemyIsSet = true;
        }
    }
}