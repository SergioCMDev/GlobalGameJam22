using System.Collections.Generic;
using System.Linq;
using App;
using App.Events;
using Presentation.Hostiles;
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
        [SerializeField] private PlayerGetResourceEvent _playerGetResourceEvent;
        protected List<TileDataEntity> tilesToAttack;
        private GameObject _enemyGameObject;
        private SoundManager _soundManager;
        private Vector3Int _attackArea;
        private bool _isActive;
        private static readonly int DeployTrigger = Animator.StringToHash("Deploy");
        private static readonly int AttackTrigger = Animator.StringToHash("Shoot");

        public Vector3Int AttackArea => _attackArea;


        public MilitaryBuildingPlacementSetter BuildingPlacementSetter => _militaryBuildingPlacementSetter;

        protected internal int AttackRingRange => _attackRingRange;

        private void Awake()
        {
            _attackArea = new Vector3Int(2 * AttackRingRange + 1, 2 * AttackRingRange + 1, 1);
            _soundManager = ServiceLocator.Instance.GetService<SoundManager>();
            _militaryBuildingAttacker.OnBuildingAttacks += OnBuildingAttacks;
            _militaryBuildingAttacker.OnAddMoneyToPlayer += AddMoneyToPlayer;
        }

        private void AddMoneyToPlayer(int quantity)
        {
            Debug.Log($"Obtenemos {quantity} {gameObject}");
            _playerGetResourceEvent.Quantity = quantity;
            _playerGetResourceEvent.Type = RetrievableResourceType.Gold;
            _playerGetResourceEvent.Fire();
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
        }

        public void ActivateBuilding()
        {
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

        public void Deactivate()
        {
            _isActive = false;
        }

        public void CleanOccupiers()
        {
            foreach (var tileInRange in tilesToAttack)
            {
                tileInRange.CleanOccupier();
            }
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