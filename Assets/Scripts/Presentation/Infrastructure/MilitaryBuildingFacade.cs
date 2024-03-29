using System.Collections.Generic;
using System.Linq;
using App.Buildings;
using App.Events;
using App.Resources;
using Presentation.Hostiles;
using Presentation.Managers;
using Presentation.Utils;
using Services.MilitaryBuilding;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public class MilitaryBuildingFacade : Building, IMilitaryBuilding
    {
        [SerializeField] private SfxSoundName _sfxWhenAttack;
        [SerializeField] private TextAsset basicAttackRangeFile;
        [SerializeField] private GameObject _particles;
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerGetResourceEvent _playerGetResourceEvent;
        [SerializeField] private PlacerBuildingView _chooserCanvas;
        [SerializeField] private List<AttackBehaviourData> _attackBehaviours;
        [SerializeField] private float cadence;
        private MilitaryBuildingType type;
        public MilitaryBuildingType Type => type;

        private MilitaryBuildingAttacker _militaryBuildingAttacker;
        private MilitaryBuildingPlacementSetter _militaryBuildingPlacementSetter;
        protected List<TileDataEntity> tilesToAttack;
        private GameObject _enemyGameObject;
        // private SoundPlayer _soundPlayer;
        private Vector3Int _attackArea;
        private bool _isActive;
        private static readonly int DeployTrigger = Animator.StringToHash("Deploy");
        private static readonly int AttackTrigger = Animator.StringToHash("Shoot");
        public AttackRangeData AttackRangeData => _attackRange;
        private AttackRangeData _attackRange;
        protected bool hasAttackOnce;

        public MilitaryBuildingPlacementSetter BuildingPlacementSetter => _militaryBuildingPlacementSetter;
        
        private void Awake()
        {
            // _soundPlayer = ServiceLocator.Instance.GetService<SoundPlayer>();
            _militaryBuildingPlacementSetter = new MilitaryBuildingPlacementSetter();
            _militaryBuildingPlacementSetter.Init(_chooserCanvas);
            _militaryBuildingAttacker = new MilitaryBuildingAttacker();
            
            _militaryBuildingAttacker.OnBuildingAttacks += OnBuildingAttacks;
            _militaryBuildingAttacker.OnAddMoneyToPlayer += AddMoneyToPlayer;
            _attackRange = FileReader.ReadFile(basicAttackRangeFile.text);
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
            // _soundPlayer.PlaySfx(_sfxWhenAttack);
        }

        protected virtual void Update()
        {
            if (!_isActive || !_militaryBuildingAttacker.CanAttack()) return;
            var enemiesToAttack = GetReachableEnemies();
            if (!enemiesToAttack.Any()) return;

            hasAttackOnce = true;

            SaveEnemiesToAttack(enemiesToAttack);
            _militaryBuildingAttacker.Attack(enemiesToAttack);
        }

        protected virtual void SaveEnemiesToAttack(List<Enemy> enemiesToAttack)
        {
        }

        private List<Enemy> GetReachableEnemies()
        {
            var reachableEnemies = tilesToAttack.FindAll(tile =>
                    tile.IsOccupied && tile.Occupier != gameObject && tile.Occupier.CompareTag("Enemy"))
                .Select(tile => tile.Occupier.GetComponent<Enemy>()).ToList();
            return reachableEnemies;
        }

        public void Init()
        {
            _militaryBuildingAttacker.Init(_attackBehaviours,cadence);
            _militaryBuildingPlacementSetter.Init(_chooserCanvas);
            CleanOccupiers();
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
            tilesToAttack?.Clear();
        }

        public bool ContainsTileToAttack(TileDataEntity tileDataEntity)
        {
            return tilesToAttack.Contains(tileDataEntity);
        }

        public void SetType(MilitaryBuildingType tilemapEventMilitaryBuildingType) 
        {
            type = tilemapEventMilitaryBuildingType;
        }
    }
}