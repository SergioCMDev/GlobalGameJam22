using Application_.Events;
using UnityEngine;

namespace Presentation
{
    public abstract class MilitaryBuilding : Building, IAttack
    {
        [SerializeField] private float _cadence, _damage;
        [SerializeField] private SfxSoundName _sfxWhenAttack;
        [SerializeField] private DamageType _damageType;
        [SerializeField] private PlaySFXEvent _playSfxEvent;
        [SerializeField] private GameObject _particles;

        private IReceiveDamage enemyToAttack;

        private float _lastTimeAttacked;

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
        
        protected override void ReceiveDamage(float receivedDamage)
        {
            Life -= receivedDamage;
            UpdateLifeSliderBar();
        }

        public void Attack(IReceiveDamage objectToAttack)
        {
            if (!CanAttack() || !CanReach(objectToAttack)) return;
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

        protected abstract bool CanReach(IReceiveDamage objectToAttack);
        protected abstract bool CanReach();


        private void Update()
        {
            if (CanAttack() && CanReach(enemyToAttack))
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
        }
    }
}