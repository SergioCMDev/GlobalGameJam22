using System.Collections.Generic;
using System.Linq;
using App.Events;
using Presentation.Hostiles;
using Presentation.Interfaces;
using Presentation.Managers;
using UnityEngine;
using Utils;

namespace Presentation.Infrastructure
{
    public class MilitaryBuildingFacade : Building
    {
        [SerializeField] private MilitaryBuildingPlacementSetter _militaryBuildingPlacementSetter;
        [SerializeField] private MilitaryBuildingAttacker _militaryBuildingAttacker;
        [SerializeField] private SfxSoundName _sfxWhenAttack;
        [SerializeField] private GameObject _particles;
        [SerializeField] private Animator _animator;
        [SerializeField] private int _attackRingRange = 1;
        [SerializeField] protected List<TileDataEntity> tilesToAttack;

        private GameObject _enemyGameObject;
        private SoundManager _soundManager;
        private Vector3Int _attackArea;
        private bool _enemyIsSet, _isActive;
        private static readonly int DeployTrigger = Animator.StringToHash("Deploy");
        private static readonly int AttackTrigger = Animator.StringToHash("Shoot");

        public Vector3Int AttackArea => _attackArea;


        public MilitaryBuildingPlacementSetter BuildingPlacementSetter => _militaryBuildingPlacementSetter;

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
            _soundManager = ServiceLocator.Instance.GetService<SoundManager>();
            _militaryBuildingAttacker.OnBuildingAttacks += OnBuildingAttacks;
        }

        private void OnBuildingAttacks()
        {
            _animator.SetTrigger(AttackTrigger);
            PlaySoundWhenAttacks();
            ThrowParticlesWhenAttacks();
        }

        private void ThrowParticlesWhenAttacks()
        {
        }

        private void PlaySoundWhenAttacks()
        {
            _soundManager.PlaySfx(_sfxWhenAttack);
        }

        private bool CanReachSomeEnemy()
        {
            return
                tilesToAttack.Any(tile => tile.IsOccupied && tile.Occupier != gameObject);
        }

        private void Update()
        {
            if (!_isActive || !_militaryBuildingAttacker.CanAttack()) return;
            var enemiesToAttack = GetReachableEnemies();
            if (!enemiesToAttack.Any()) return;
            _militaryBuildingAttacker.Attack(enemiesToAttack);
        }

        private List<Enemy> GetReachableEnemies()
        {
            var reachableEnemies = tilesToAttack.FindAll(tile =>
                    tile.IsOccupied && tile.Occupier != gameObject  && tile.Occupier.CompareTag("Enemy"))
                .Select(tile => tile.Occupier.GetComponent<Enemy>()).ToList();
            return reachableEnemies;
        }

        public void Init()
        {
            _militaryBuildingAttacker.Init();
            _enemyIsSet = true;
            _isActive = true;
        }

        public void Deploy()
        {
            _animator.SetTrigger(DeployTrigger);
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

        public void SetTilesToAttack(List<TileDataEntity> tileDataEntities)
        {
            tilesToAttack = tileDataEntities;
        }

        public void ClearAttackTiles()
        {
            tilesToAttack.Clear();
        }
    }
}