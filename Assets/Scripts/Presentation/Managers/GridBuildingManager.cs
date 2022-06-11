using System;
using System.Collections.Generic;
using System.Linq;
using App;
using App.Events;
using Presentation.Infrastructure;
using Presentation.Structs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Presentation.Managers
{
    [Serializable]
    public class TileDataEntity
    {
        public Dictionary<Tilemap, TilemapColours> TilemapColours;
        public TileBase TileBase;
        public TileBase TileBaseWorld;
        public Vector3Int GridPosition;
        public bool IsOccupied;
        public GameObject Occupier;
        public Vector3 WorldPosition;
        public bool Locked;
        public TilePosition TilePosition; //REFACTOR

        public TileDataEntity()
        {
            Locked = false;
            TilemapColours = new Dictionary<Tilemap, TilemapColours>();
        }

        public void CleanOccupier()
        {
            IsOccupied = false;
            Occupier = null;
        }
    }

    [Serializable]
    public class BuildingPositionTuple
    {
        public Vector3Int positionInGrid;
        public Building cityBuilding;
    }

    public class GridBuildingManager : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap, _buildingTilemap, _weaponRangeTilemap;
        [SerializeField] private Grid _grid;
        [SerializeField] private Transform _buildingParent;
        [SerializeField] private SaveBuildingEvent saveBuildingEvent;
        [SerializeField] private Tile _red, white, green, _purple;

        private Dictionary<TileType, TileBase> _tileTypeBase = new Dictionary<TileType, TileBase>();

        // public IDictionary<Vector3, Vector3Int> world; //REMOVE
        private GameObject _building;
        private Vector3Int _currentObjectPosition, _currentBuildingArea, _originalBuildingArea;

        private MilitaryBuildingFacade _buildingFacadeComponent;

        private readonly List<SetBuildingData> _savedBuildings = new List<SetBuildingData>();
        private readonly List<TileDataEntity> tileDatasBuilding = new List<TileDataEntity>();
        private readonly List<TileDataEntity> tileDatasAttack = new List<TileDataEntity>();

        private Dictionary<Vector3Int, TileDataEntity> _worldTileDictionaryBuildingTilemap = new();

        [SerializeField] private bool _showAttackZone;

        public event Action OnPlayerHasCanceledSetBuildingOnGrid;
        public event Action<MilitaryBuildingFacade> OnPlayerHasSetBuildingOnGrid;

        public Dictionary<Vector3Int, TileDataEntity> WorldTileDictionary => _worldTileDictionaryBuildingTilemap;

        private void Awake()
        {
            // string tilePath = @"Tiles\";
            //TODO LOAD FROM RESOURCES
            _tileTypeBase.Add(TileType.Empty, null);
            _tileTypeBase.Add(TileType.White, white);
            // tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
            _tileTypeBase.Add(TileType.Green, green);
            // tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
            // tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
            _tileTypeBase.Add(TileType.Red, _red);
            _tileTypeBase.Add(TileType.Blue, _purple);

            ReadWorld();


            HideTemporalTileMap();
        }

        public void StatusDrawingTurretRange(SetStatusDrawingTurretRangesEvent setStatusDrawingTurretRangesEvent)
        {
            if (setStatusDrawingTurretRangesEvent.drawingStatus)
            {
                _weaponRangeTilemap.gameObject.SetActive(true);
                DrawMilitaryBuildings();
                return;
            } 
            _weaponRangeTilemap.gameObject.SetActive(false);
            //TODO Limpiar tiles pintadas, relacionado con TODO e evitar usar BOUNDSINT
        }

        public void AllowPlayerToSetBuildingInTilemap(AllowPlayerToSetBuildingInTilemapEvent tilemapEvent)
        {
            ShowTemporalTileMap();
            LoadMilitaryBuildings();
            _building = Instantiate(tilemapEvent.Prefab, _buildingParent); //GET POOL
            _buildingFacadeComponent = _building.GetComponent<MilitaryBuildingFacade>();
            _buildingFacadeComponent.Initialize();
            _buildingFacadeComponent.Select();
            _buildingFacadeComponent.BuildingPlacementSetter.SetStatusChooserCanvas(true);
            _buildingFacadeComponent.BuildingPlacementSetter.OnCancelTakingPlace += CancelTakingPlace;
            _buildingFacadeComponent.BuildingPlacementSetter.OnBuildingTriesToTakePlace += BuildingTriesToTakePlace;
            _currentBuildingArea = _buildingFacadeComponent.Area;
            _originalBuildingArea = _buildingFacadeComponent.Area;
            _building.transform.position = _tilemap.GetCellCenterWorld(new Vector3Int());
        }

        private void CancelTakingPlace()
        {
            _buildingFacadeComponent.BuildingPlacementSetter.OnCancelTakingPlace -= CancelTakingPlace;
            _buildingFacadeComponent.BuildingPlacementSetter.OnBuildingTriesToTakePlace -= BuildingTriesToTakePlace;

            HideTemporalTileMap();
            _buildingFacadeComponent.Deselect();
            Destroy(_building);
            ClearPreviousPaintedArea();

            OnPlayerHasCanceledSetBuildingOnGrid.Invoke();
        }

        private void BuildingTriesToTakePlace()
        {
            var colours = tileDatasBuilding.Select(x => x.TilemapColours[_buildingTilemap].CurrentColour).ToList();
            if (!CanBePlacedHere(colours)) return;
            SetBuildingInGrid();
        }

        private void SetBuildingInGrid()
        {
            SaveBuilding();
            //TODO REMOVE THE ATTACK ZONE TO LEAVE WHITE ZONE SURROUNDING THE BUILDING
            _buildingFacadeComponent.BuildingPlacementSetter.OnCancelTakingPlace -= CancelTakingPlace;
            _buildingFacadeComponent.BuildingPlacementSetter.OnBuildingTriesToTakePlace -= BuildingTriesToTakePlace;
            _buildingFacadeComponent.BuildingPlacementSetter.SetStatusChooserCanvas(false);
            ClearPreviousPaintedArea();
            HideTemporalTileMap();
            WorldTileDictionary[_currentObjectPosition].IsOccupied = true;
            WorldTileDictionary[_currentObjectPosition].Occupier = _buildingFacadeComponent.gameObject;
            _buildingFacadeComponent.Deselect();

            var attackTiles =
                GetAttackTilesOfBuilding(_currentObjectPosition, _buildingFacadeComponent, _buildingTilemap);
            _currentObjectPosition = Vector3Int.zero;
            _buildingFacadeComponent.SetTilesToAttack(attackTiles);
            OnPlayerHasSetBuildingOnGrid?.Invoke(_buildingFacadeComponent);
            _buildingFacadeComponent = null;
            saveBuildingEvent.Instance = _building;
            saveBuildingEvent.Fire();
            tileDatasBuilding.Clear();
            tileDatasAttack.Clear();
        }

        private List<TileDataEntity> GetAttackTilesOfBuilding(Vector3Int buildingPosition,
            MilitaryBuildingFacade militaryBuildingFacade, Tilemap tilemapBuilding)
        {
            var offset = Vector3Int.up * militaryBuildingFacade.AttackRingRange +
                         Vector3Int.right * militaryBuildingFacade.AttackRingRange;
            var temporalObjectArea = GetObjectArea(buildingPosition - offset, militaryBuildingFacade.AttackArea);
            var attackArray = GetTilesBlock(temporalObjectArea, tilemapBuilding, WorldTileDictionary);
            militaryBuildingFacade.SetTilesToAttack(attackArray);
            return attackArray;
        }

        private void ReadWorld()
        {
            // world = new Dictionary<Vector3, Vector3Int>();
            for (int n = _tilemap.cellBounds.xMin; n < _tilemap.cellBounds.xMax; n++)
            {
                for (int p = _tilemap.cellBounds.yMin; p < _tilemap.cellBounds.yMax; p++)
                {
                    Vector3Int gridPosition = (new Vector3Int(n, p, (int)_tilemap.transform.position.y));
                    Vector3 worldPosition = _tilemap.CellToWorld(gridPosition);
                    // Debug.Log($"Grid POsition {gridPosition}");
                    // if (!_tilemap.HasTile(gridPosition) || !_buildingTilemap.HasTile(gridPosition)) continue;
                    // Debug.Log($"Grid POsition OK {gridPosition} ");

                    var tilemapColourDictionary = new Dictionary<Tilemap, TilemapColours>();
                    if (_buildingTilemap.HasTile(gridPosition))
                    {
                        tilemapColourDictionary.Add(_buildingTilemap, new TilemapColours()
                        {
                            PreviousColour = GetCurrentTileType(_buildingTilemap.GetTile(gridPosition)),
                            CurrentColour = GetCurrentTileType(_buildingTilemap.GetTile(gridPosition)),
                            OriginalColour = GetCurrentTileType(_buildingTilemap.GetTile(gridPosition)),
                        });
                    }

                    // if (_weaponRangeTilemap.HasTile(gridPosition))
                    // {
                    tilemapColourDictionary.Add(_weaponRangeTilemap, new TilemapColours()
                    {
                        PreviousColour = GetCurrentTileType(_weaponRangeTilemap.GetTile(gridPosition)),
                        CurrentColour = GetCurrentTileType(_weaponRangeTilemap.GetTile(gridPosition)),
                        OriginalColour = GetCurrentTileType(_weaponRangeTilemap.GetTile(gridPosition)),
                    });
                    // }
                    if (_tilemap.HasTile(gridPosition))
                    {
                        WorldTileDictionary.Add(gridPosition, new TileDataEntity()
                        {
                            WorldPosition = worldPosition,
                            GridPosition = gridPosition,
                            Occupier = null,
                            IsOccupied = false,
                            TilemapColours = tilemapColourDictionary,
                            TileBase = _buildingTilemap.GetTile(gridPosition),
                            TileBaseWorld = _tilemap.GetTile(gridPosition)
                        });
                    }

                    // world.Add(worldPosition, gridPosition);
                }
            }
        }

        private Vector3Int GetGridPositionByMouse(Vector3 inputMousePosition)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(inputMousePosition);
            Vector3Int gridPosition = _grid.LocalToCell(mousePosition);
            return gridPosition;
        }

        private void ShowTemporalTileMap()
        {
            _buildingTilemap.gameObject.SetActive(true);
            _weaponRangeTilemap.gameObject.SetActive(true);
        }

        private void LoadMilitaryBuildings()
        {
            DrawMilitaryBuildings();

            foreach (var buildingData in _savedBuildings)
            {
                WorldTileDictionary[buildingData.position].Occupier =
                    buildingData.buildingFacadeComponent.gameObject;
                WorldTileDictionary[buildingData.position].IsOccupied = true;
            }

            tileDatasAttack.Clear();
            tileDatasBuilding.Clear();
        }

        private void DrawMilitaryBuildings()
        {
            foreach (var buildingData in _savedBuildings)
            {
                SetBuildingRelativeZone(buildingData.buildingFacadeComponent, buildingData.position, TileType.Red,
                    false);
            }
        }

        
        
        private void SetBuildingRelativeZone(MilitaryBuildingFacade militaryBuildingFacade,
            Vector3Int gridPosition,
            TileType buildingColour = TileType.Green, bool canBeCleaned = true)
        {
            tileDatasBuilding.Clear();
            tileDatasAttack.Clear();
            //TODO Encontrar manera de hace esto sin usar BOUNDSINT
            SetAttackZone(militaryBuildingFacade, gridPosition, canBeCleaned);
            var currentTileAttackArray = CopyFromTileDataToArray(tileDatasAttack);
            var tileAttackPositions = GetTilePositionsOfTileData(tileDatasAttack);

            SetBuildingZone(buildingColour, gridPosition);
            var currentTileBuildingArray = CopyFromTileDataToArray(tileDatasBuilding);
            var tileBuildingPositions = GetTilePositionsOfTileData(tileDatasBuilding);

            SetTilesInTilemap(tileAttackPositions, currentTileAttackArray, _weaponRangeTilemap);
            SetTilesInTilemap(tileBuildingPositions, currentTileBuildingArray, _buildingTilemap);
        }

        private Vector3Int[] GetTilePositionsOfTileData(List<TileDataEntity> tileData)
        {
            var positions = tileData.Select(x => x.GridPosition).ToArray();
            return positions;
        }

        private void HideTemporalTileMap()
        {
            _buildingTilemap.gameObject.SetActive(false);
            _weaponRangeTilemap.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_buildingTilemap.gameObject.activeInHierarchy || !_building) return;
            if (!Input.GetMouseButton(0)) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;

            var buildingGridPosition = GetGridPositionByMouse(Input.mousePosition);
            if (buildingGridPosition == _currentObjectPosition ||
                !PositionIsInsideBuildingTilemapGrid(buildingGridPosition)) return;

            ClearPreviousPaintedArea();
            _building.transform.position = _tilemap.GetCellCenterLocal(buildingGridPosition);

            var temporalObjectArea = GetObjectArea(buildingGridPosition, _currentBuildingArea);
            var buildingArray =
                GetTilesBlock(temporalObjectArea, _buildingTilemap, WorldTileDictionary);

            if (buildingArray.Count <= 0) return;
            SetColourOfBuildingTiles(buildingArray, _currentBuildingArea);
            _currentObjectPosition = buildingGridPosition;
            var colours = tileDatasBuilding.Select(x => x.TilemapColours[_buildingTilemap].CurrentColour).ToList();
            if (CanBePlacedHere(colours) && _showAttackZone)
            {
                Debug.Log("Can Place");
                _currentBuildingArea = _buildingFacadeComponent.AttackArea;

                SetBuildingRelativeZone(_buildingFacadeComponent, _currentObjectPosition);
                return;
            }

            var buildingArea = GetObjectArea(buildingGridPosition, temporalObjectArea.size);
            SetTilesInTilemap(buildingArea, CopyFromTileDataToArray(tileDatasBuilding), _buildingTilemap);
        }

        private bool PositionIsInsideBuildingTilemapGrid(Vector3Int buildingGridPosition)
        {
            return _worldTileDictionaryBuildingTilemap.ContainsKey(buildingGridPosition) &&
                   _worldTileDictionaryBuildingTilemap[buildingGridPosition].TilemapColours
                       .ContainsKey(_buildingTilemap);
        }

        private TileDataEntity GetTileDataByGridPosition(List<TileDataEntity> tileDataEntities,
            Vector3Int currentObjectPosition) =>
            tileDataEntities.SingleOrDefault(x => x.GridPosition == currentObjectPosition);

        private void SetBuildingZone(TileType tileTypeBuilding, Vector3Int buildingPosition)
        {
            var tileData = WorldTileDictionary[buildingPosition];
            var tileDataColour = GetTilemapColour(tileData, _buildingTilemap);

            tileDataColour.CurrentColour = tileTypeBuilding;
            tileData.GridPosition = buildingPosition;
            tileData.TileBase = _tileTypeBase[tileTypeBuilding];
            tileData.TilemapColours[_buildingTilemap] = tileDataColour;
            tileDatasBuilding.Add(tileData);
        }

        private void SetAttackZone(MilitaryBuildingFacade militaryBuildingFacade, Vector3Int buildingPosition,
            bool canBeCleaned)
        {
            tileDatasAttack.Clear();

            var attackArray =
                GetAttackTilesOfBuilding(buildingPosition, militaryBuildingFacade, _weaponRangeTilemap);
            if (attackArray.Any(x => x.GridPosition == buildingPosition))
            {
                var buildingTile = attackArray.Single(x => x.GridPosition == buildingPosition);
                attackArray.Remove(buildingTile);
            }

            SetColourOfAttackZone(attackArray, canBeCleaned);
        }

        private void SetColourOfAttackZone(List<TileDataEntity> attackArray, bool canBeCleaned)
        {
            for (int i = 0; i < attackArray.Count; i++)
            {
                var colours = attackArray[i].TilemapColours[_weaponRangeTilemap];
                if (attackArray[i].IsOccupied)
                {
                    attackArray[i].TileBase = _tileTypeBase[TileType.Red];
                    colours.CurrentColour = TileType.Red;
                }
                else if (attackArray[i].TilemapColours[_weaponRangeTilemap].CurrentColour == TileType.Red)
                {
                    attackArray[i].TileBase = _tileTypeBase[TileType.Red];
                    colours.CurrentColour = TileType.Red;
                    colours.PreviousColour = TileType.Red;
                }
                else
                {
                    attackArray[i].TileBase = _tileTypeBase[TileType.Blue];
                    colours.CurrentColour = TileType.Blue;
                    if (!canBeCleaned)
                        colours.PreviousColour = TileType.Blue;
                }

                attackArray[i].TilemapColours[_weaponRangeTilemap] = colours;
                AddTemporalTileData(attackArray[i], tileDatasAttack);
            }
        }

        private bool CanBePlacedHere(List<TileType> tileArray)
        {
            return tileArray.Any(x => x == TileType.Green || x == TileType.Blue);
            // return _worldTileDictionary[_currentObjectPosition].IsOccupied;
        }

        private void SetColourOfBuildingTiles(List<TileDataEntity> baseArray, Vector3Int buildingArea)
        {
            for (var i = 0; i < buildingArea.x * buildingArea.y * buildingArea.z; i++)
            {
                var tilemapColour = GetTilemapColour(baseArray[i], _buildingTilemap);
                if (!baseArray[i].IsOccupied && tilemapColour.CurrentColour == TileType.White ||
                    tilemapColour.CurrentColour == TileType.Blue && tilemapColour.OriginalColour == TileType.White)
                {
                    tilemapColour.CurrentColour = TileType.Green;
                    baseArray[i].TileBase = _tileTypeBase[TileType.Green];
                    baseArray[i].TilemapColours[_buildingTilemap] = tilemapColour;
                    AddTemporalTileData(baseArray[i], tileDatasBuilding);
                    continue;
                }

                tileDatasBuilding.Clear();
                for (var x = 0; i < buildingArea.x * buildingArea.y * buildingArea.z; i++)
                {
                    tilemapColour.CurrentColour = TileType.Red;
                    baseArray[x].TileBase = _tileTypeBase[TileType.Red];

                    AddTemporalTileData(baseArray[x], tileDatasBuilding);
                }

                break;
            }
        }

        private TilemapColours GetTilemapColour(TileDataEntity baseArray, Tilemap tilemap)
        {
            return baseArray.TilemapColours[tilemap];
        }

        private void AddTemporalTileData(TileDataEntity tileDataEntity, List<TileDataEntity> tileDataEntities)
        {
            if (tileDataEntity == null) return;
            tileDataEntities.Add(tileDataEntity);
        }

        private void ClearPreviousPaintedArea()
        {
            ClearPaintedTiles(tileDatasBuilding, _buildingTilemap);
            ClearAttackPaintedTiles(tileDatasAttack, _weaponRangeTilemap);

            _currentBuildingArea = _originalBuildingArea;
        }

        private void ClearAttackPaintedTiles(List<TileDataEntity> tilesToClear, Tilemap tilemap)
        {
            if (tilesToClear.Count == 0) return;

            for (var index = 0; index < tilesToClear.Count; index++)
            {
                var tile = tilesToClear[index];
                var tileMapColour = GetTilemapColour(tile, tilemap);
                switch (tileMapColour.CurrentColour)
                {
                    case TileType.Red when tileMapColour.PreviousColour == TileType.Empty:
                    case TileType.Red when tileMapColour.PreviousColour == TileType.Red:
                    case TileType.Red when tileMapColour.PreviousColour == TileType.White:
                    case TileType.Red when tileMapColour.PreviousColour == TileType.Blue:
                    case TileType.Blue when tileMapColour.PreviousColour == TileType.Empty:
                        tile.TileBase = _tileTypeBase[tileMapColour.PreviousColour];
                        tileMapColour.CurrentColour = tileMapColour.PreviousColour;
                        break;
                    case TileType.Blue when tileMapColour.PreviousColour == TileType.Blue:
                        tile.TileBase = _tileTypeBase[TileType.Blue];
                        tileMapColour.CurrentColour = TileType.Blue;
                        tileMapColour.PreviousColour = TileType.Blue;
                        break;
                    default:
                        tile.TileBase = _tileTypeBase[TileType.Empty];
                        tileMapColour.CurrentColour = TileType.White;
                        tileMapColour.PreviousColour = TileType.White;
                        break;
                }

                tilesToClear[index] = tile;
            }

            var filledTiles = CopyFromTileDataToArray(tilesToClear);
            var tilePositions = GetTilePositionsOfTileData(tilesToClear);
            _buildingFacadeComponent.ClearAttackTiles();

            SetTilesInTilemap(tilePositions, filledTiles, tilemap);
            tilesToClear.Clear();
        }

        private void ClearPaintedTiles(List<TileDataEntity> tilesToClear, Tilemap tilemap)
        {
            if (tilesToClear.Count == 0) return;

            for (var index = 0; index < tilesToClear.Count; index++)
            {
                var tile = tilesToClear[index];
                var tileMapColour = GetTilemapColour(tile, tilemap);

                switch (tileMapColour.CurrentColour)
                {
                    case TileType.Blue when tileMapColour.PreviousColour == TileType.Blue:
                        tile.TileBase = _tileTypeBase[TileType.Empty];
                        break;
                    case TileType.Green:
                    case TileType.Red when tileMapColour.PreviousColour == TileType.Empty:
                    case TileType.Red when tileMapColour.PreviousColour == TileType.Red:
                    case TileType.Red when tileMapColour.PreviousColour == TileType.White:
                        tile.TileBase = _tileTypeBase[tileMapColour.PreviousColour];
                        tileMapColour.CurrentColour = tileMapColour.PreviousColour;
                        break;
                    default:
                        tile.TileBase = _tileTypeBase[TileType.White];
                        tileMapColour.CurrentColour = TileType.White;
                        tileMapColour.PreviousColour = TileType.White;
                        break;
                }

                tilesToClear[index].TilemapColours[tilemap] = tileMapColour;
                tilesToClear[index] = tile;
            }

            var filledTiles = CopyFromTileDataToArray(tilesToClear);
            var tilePositions = GetTilePositionsOfTileData(tilesToClear);

            SetTilesInTilemap(tilePositions, filledTiles, tilemap);
            tilesToClear.Clear();
        }

        private TileBase[] CopyFromTileDataToArray(List<TileDataEntity> tileData)
        {
            var filledTiles = new TileBase[tileData.Count];
            for (var index = 0; index < tileData.Count; index++)
            {
                filledTiles[index] = tileData[index].TileBase;
            }

            return filledTiles;
        }

        private BoundsInt GetObjectArea(Vector3Int gridPosition, Vector3Int sizeArea)
        {
            BoundsInt objectArea = new BoundsInt(gridPosition, sizeArea);
            return objectArea;
        }

        private void SetTilesInTilemap(BoundsInt buildingArea, TileBase[] tileArray, Tilemap tilemap)
        {
            tilemap.SetTilesBlock(buildingArea, tileArray);
        }

        private void SetTilesInTilemap(Vector3Int[] positions, TileBase[] tileArray, Tilemap tilemap)
        {
            tilemap.SetTiles(positions, tileArray);
        }

        private TileType GetCurrentTileType(TileBase tile)
        {
            if (tile == _tileTypeBase[TileType.Empty])
            {
                return TileType.Empty;
            }

            if (tile == _tileTypeBase[TileType.Green])
            {
                return TileType.Green;
            }

            if (tile == _tileTypeBase[TileType.Red])
            {
                return TileType.Red;
            }

            return tile == _tileTypeBase[TileType.Blue] ? TileType.Blue : TileType.White;
        }

        private void SaveBuilding()
        {
            _savedBuildings.Add(new SetBuildingData()
                {
                    building = _building,
                    buildingFacadeComponent = _buildingFacadeComponent,
                    position = _currentObjectPosition
                }
            );
        }

        private List<TileDataEntity> GetTilesBlock(BoundsInt area, Tilemap tilemap,
            Dictionary<Vector3Int, TileDataEntity> worldDictionary)
        {
            var list = new List<TileDataEntity>();

            foreach (var v in area.allPositionsWithin)
            {
                var pos = new Vector3Int(v.x, v.y, 0);
                if (!worldDictionary.ContainsKey(pos) || list.Contains(worldDictionary[pos])) continue;

                list.Add(worldDictionary[pos]);
            }

            return list;
        }

        public void ObjectHasMovedToNewTile(ObjectHasMovedToNewTileEvent tileEvent)
        {
            ObjectHasMovedToNewTile(tileEvent.Occupier, tileEvent.GridPositions);
        }

        public void ObjectHasMovedToNewTile(GameObject occupier, GridPositionTuple tuplePosition)
        {
            if (tuplePosition.OldGridPosition == tuplePosition.NewGridPosition ||
                !WorldTileDictionary.ContainsKey(tuplePosition.NewGridPosition)) return;
            Debug.Log($"NUEVA TILE {tuplePosition.NewGridPosition}");

            WorldTileDictionary[tuplePosition.NewGridPosition].IsOccupied = true;
            WorldTileDictionary[tuplePosition.NewGridPosition].Occupier = occupier;

            WorldTileDictionary[tuplePosition.OldGridPosition].CleanOccupier();
        }

        public void SetCitiesInGrid(List<BuildingPositionTuple> buildingPositionTuples)
        {
            foreach (var buildingPosition in buildingPositionTuples)
            {
                _worldTileDictionaryBuildingTilemap[buildingPosition.positionInGrid].Occupier =
                    buildingPosition.cityBuilding.gameObject;
                _worldTileDictionaryBuildingTilemap[buildingPosition.positionInGrid].IsOccupied = true;
            }
        }
    }
}