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
        [SerializeField] private float _cadence, _damage;
        [SerializeField] private GameObject _particles;
        [SerializeField] private int _attackRingRange = 1;

        private IReceiveDamage enemyToAttack;
        private GameObject enemyGameObject;
        public Vector3Int AttackArea => _attackArea;
        private Vector3Int _attackArea;

        private bool _isActive;

        private bool _enemyIsSet;
        // public float DistanceToAttack => _distanceToAttack;

        public MilitaryBuildingPlacementSetter BuildingPlacementSetter => _militaryBuildingPlacementSetter;

        public MilitaryBuildingAttacker BuildingAttacker => _militaryBuildingAttacker;

        protected internal int AttackRingRange
        {
            get => _attackRingRange;
            set => _attackRingRange = value;
        }


        public override void ReceiveDamage(float receivedDamage)
        {
            Life -= receivedDamage;
            UpdateLifeSliderBar();
        }


        private void Awake()
        {
            _attackArea = new Vector3Int(2 * AttackRingRange + 1, 2 * AttackRingRange + 1, 1);
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
            // BuildingAttacker.Attack(enemyToAttack);
        }

        public void SetEnemyToAttack(GameObject enemy)
        {
            if (enemy == null) return;
            enemyToAttack = enemy.GetComponent<IReceiveDamage>();
            enemyGameObject = enemy;
            BuildingAttacker.Init(enemyGameObject, _cadence, _damage);
            _enemyIsSet = true;
            _isActive = true;
        }

        public void Select()
        {
            SpriteRenderer.color = colorWithTransparency;
        }

        public void Deselect()
        {
            SpriteRenderer.color = originalColor;
        }


        public void Stop()
        {
            _isActive = false;
        }
    }
}