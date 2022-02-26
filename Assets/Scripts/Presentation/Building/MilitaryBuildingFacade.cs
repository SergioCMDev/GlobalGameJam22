using Application_.Events;
using Presentation.Interfaces;
using Presentation.Weapons;
using UnityEngine;

namespace Presentation.Building
{
    public enum AttackRangeType
    {
        Ring,
        Square
    }

    public abstract class MilitaryBuildingFacade : Building
    {
        [SerializeField] private MilitaryBuildingPlacementSetter _militaryBuildingPlacementSetter;
        [SerializeField] private MilitaryBuildingAttacker _militaryBuildingAttacker;
        [SerializeField] private SfxSoundName _sfxWhenAttack;
        [SerializeField] private PlaySFXEvent _playSfxEvent;
        [SerializeField] private float _cadence, _damage, _distanceToAttack;
        [SerializeField] private GameObject _particles;
        private IReceiveDamage enemyToAttack;
        private GameObject enemyGameObject;

        private bool _isActive;
        private bool _enemyIsSet;
        public float DistanceToAttack => _distanceToAttack;

        public MilitaryBuildingPlacementSetter BuildingPlacementSetter => _militaryBuildingPlacementSetter;

        public MilitaryBuildingAttacker BuildingAttacker => _militaryBuildingAttacker;


        public override void ReceiveDamage(float receivedDamage)
        {
            Life -= receivedDamage;
            UpdateLifeSliderBar();
        }

        private void Awake()
        {
            BuildingAttacker.OnBuildingAttacks += OnBuildingAttacks;
        }

        private void OnBuildingAttacks()
        {
            PlaySoundWhenAttacks();
            ThrowParticlesWhenAttacks();
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
            if (!_isActive || !_enemyIsSet || !BuildingAttacker.CanAttack() ||
                !CanReach(enemyGameObject)) return;

            Debug.Log("ATTACK");
            BuildingAttacker.Attack(enemyToAttack);
        }

        public void SetEnemyToAttack(GameObject enemy)
        {
            if (enemy == null) return;
            enemyToAttack = enemy.GetComponent<IReceiveDamage>();
            enemyGameObject = enemy;
            BuildingAttacker.Init(enemyGameObject, _cadence, _distanceToAttack, _damage);
            _enemyIsSet = true;
            _isActive = true;
        }

        public void Select()
        {
            _spriteRenderer.color = colorWithTransparency;
        }

        public void Deselect()
        {
            _spriteRenderer.color = originalColor;
        }


        public void Stop()
        {
            _isActive = false;
        }
    }
}