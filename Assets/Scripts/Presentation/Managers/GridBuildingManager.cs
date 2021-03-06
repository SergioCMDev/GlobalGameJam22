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
        [SerializeField] private Tilemap _tilemap, _buildingTilemap, _weaponRangeTilemap, _weaponToSetRangeTilemap;
        [SerializeField] private Grid _grid;
        [SerializeField] private Transform _buildingParent;
        [SerializeField] private SaveBuildingEvent saveBuildingEvent;
        [SerializeField] private Tile _red, white, green, blue, purple, empty;

        private Dictionary<TileType, TileBase> _tileTypeBase = new();
        private GameObject _building;
        private Vector3Int _currentObjectPosition, _currentBuildingArea, _originalBuildingArea;

        private MilitaryBuildingFacade _buildingFacadeComponent;

        private readonly List<SetBuildingData> _savedBuildings = new();

        private Dictionary<Vector3Int, TileDataEntity> _worldTileDictionaryBuildingTilemap = new();

        [SerializeField] private bool _showAttackZone;
        private List<TileDataEntity> _temporalRangeTilesToDraw = new();
        private List<TileDataEntity> _temporalBuildingCenterTileToDraw = new();

        public event Action OnPlayerHasCanceledSetBuildingOnGrid;
        public event Action<MilitaryBuildingFacade> OnPlayerHasSetBuildingOnGrid;

        public Dictionary<Vector3Int, TileDataEntity> WorldTileDictionary => _worldTileDictionaryBuildingTilemap;

        private void Awake()
        {
            // string tilePath = @"Tiles\";
            //TODO LOAD FROM RESOURCES
            _tileTypeBase.Add(TileType.Empty, empty);
            _tileTypeBase.Add(TileType.White, white);
            // tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
            _tileTypeBase.Add(TileType.Green, green);
            // tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
            // tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
            _tileTypeBase.Add(TileType.Red, _red);
            _tileTypeBase.Add(TileType.Blue, blue);
            _tileTypeBase.Add(TileType.Purple, purple);

            ReadWorld();


            HideTemporalTileMap();
        }

        //Used by ShowRange Button
        public void StatusDrawingTurretRange(SetStatusDrawingTurretRangesEvent setStatusDrawingTurretRangesEvent)
        {
            if (setStatusDrawingTurretRangesEvent.drawingStatus)
            {
                _weaponRangeTilemap.gameObject.SetActive(true);
                DrawRangeSavedMilitaryBuildings();
                return;
            }
            ClearAttackPaintedTiles(_temporalRangeTilesToDraw, _weaponRangeTilemap);
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
            _buildingFacadeComponent.SetType(tilemapEvent.militaryBuildingType);
            _buildingFacadeComponent.Select();
            _buildingFacadeComponent.BuildingPlacementSetter.SetStatusChooserCanvas(true);
            _buildingFacadeComponent.BuildingPlacementSetter.OnCancelTakingPlace += CancelTakingPlace;
            _buildingFacadeComponent.BuildingPlacementSetter.OnBuildingTriesToTakePlace += BuildingTriesToTakePlace;
            _currentBuildingArea = _buildingFacadeComponent.Area;
            _originalBuildingArea = _buildingFacadeComponent.Area;
            _building.transform.position = _tilemap.GetCellCenterWorld(new Vector3Int());
        }

        public void CancelTakingPlace()
        {
            _buildingFacadeComponent.BuildingPlacementSetter.OnCancelTakingPlace -= CancelTakingPlace;
            _buildingFacadeComponent.BuildingPlacementSetter.OnBuildingTriesToTakePlace -= BuildingTriesToTakePlace;

            HideTemporalTileMap();
            _buildingFacadeComponent.Deselect();
            ClearPreviousPaintedArea();
            _buildingFacadeComponent.ClearAttackTiles();

            Destroy(_building);

            OnPlayerHasCanceledSetBuildingOnGrid?.Invoke();
        }

        private void BuildingTriesToTakePlace()
        {
            var colours = _temporalBuildingCenterTileToDraw.Select(x => x.TilemapColours[_buildingTilemap].CurrentColour)
                .ToList();
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
            _buildingFacadeComponent.ClearAttackTiles();
            // _temporalRangeTilesToDraw[0].GridPosition
            foreach (var tileDataEntity in _temporalRangeTilesToDraw)
            {
                var tilemapColoursToSet = tileDataEntity.TilemapColours[_weaponToSetRangeTilemap];
                var currentColour = tilemapColoursToSet.CurrentColour;
                tileDataEntity.TilemapColours[_weaponRangeTilemap] = new TilemapColours(currentColour);
               
                WorldTileDictionary[tileDataEntity.GridPosition].TilemapColours = tileDataEntity.TilemapColours;
            }
            ClearPreviousPaintedArea();
            HideTemporalTileMap();
            WorldTileDictionary[_currentObjectPosition].IsOccupied = true;
            WorldTileDictionary[_currentObjectPosition].Occupier = _buildingFacadeComponent.gameObject;
            _buildingFacadeComponent.Deselect();

            var attackTiles =
                GetAttackTilesOfBuilding(_currentObjectPosition, _buildingFacadeComponent);
            _currentObjectPosition = Vector3Int.zero;
            _buildingFacadeComponent.SetTilesToAttack(attackTiles);
            OnPlayerHasSetBuildingOnGrid?.Invoke(_buildingFacadeComponent);
            _buildingFacadeComponent = null;
            saveBuildingEvent.Instance = _building;
            saveBuildingEvent.Fire();
        }

        private List<TileDataEntity> GetAttackTilesOfBuilding(Vector3Int buildingPosition,
            MilitaryBuildingFacade militaryBuildingFacade)
        {
            var offset = Vector3Int.up * militaryBuildingFacade.AttackRingRange +
                         Vector3Int.right * militaryBuildingFacade.AttackRingRange;
            var temporalObjectArea = GetObjectArea(buildingPosition - offset, militaryBuildingFacade.AttackArea);
            var attackArray = GetTilesBlockAttacking(temporalObjectArea);
            militaryBuildingFacade.SetTilesToAttack(attackArray);
            return attackArray;
        }

        private void ReadWorld()
        {
            for (int n = _tilemap.cellBounds.xMin; n < _tilemap.cellBounds.xMax; n++)
            {
                for (int p = _tilemap.cellBounds.yMin; p < _tilemap.cellBounds.yMax; p++)
                {
                    Vector3Int gridPosition = (new Vector3Int(n, p, (int)_tilemap.transform.position.y));
                    Vector3 worldPosition = _tilemap.CellToWorld(gridPosition);

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

                    tilemapColourDictionary.Add(_weaponRangeTilemap, new TilemapColours()
                    {
                        PreviousColour = GetCurrentTileType(_tileTypeBase[TileType.Empty]),
                        CurrentColour = GetCurrentTileType(_tileTypeBase[TileType.Empty]),
                        OriginalColour = GetCurrentTileType(_tileTypeBase[TileType.Empty]),
                    });

                    tilemapColourDictionary.Add(_weaponToSetRangeTilemap, new TilemapColours()
                    {
                        PreviousColour =  GetCurrentTileType(_tileTypeBase[TileType.Empty]),
                        CurrentColour =  GetCurrentTileType(_tileTypeBase[TileType.Empty]),
                        OriginalColour =  GetCurrentTileType(_tileTypeBase[TileType.Empty]),
                    });

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
                }
            }
        }

        private Vector3Int GetGridPositionByMouse(Vector3 inputMousePosition)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(inputMousePosition);
            Vector3Int gridPosition = _grid.LocalToCell(mousePosition);
            return gridPosition;
        }

        private void LoadMilitaryBuildings()
        {
            DrawRangeSavedMilitaryBuildings();
            DrawPositionCenterSavedMilitaryBuildings();

            foreach (var buildingData in _savedBuildings)
            {
                WorldTileDictionary[buildingData.position].Occupier =
                    buildingData.buildingFacadeComponent.gameObject;
                WorldTileDictionary[buildingData.position].IsOccupied = true;
            }
        }

        private void DrawRangeSavedMilitaryBuildings()
        {
            var tilesToDraw = new List<TileDataEntity>();
            foreach (var buildingData in _savedBuildings)
            {
                var tilesOfBuilding = DrawRangeOfMilitaryBuilding(buildingData.buildingFacadeComponent, buildingData.position,
                    false, _weaponRangeTilemap);
                tilesToDraw.AddRange(tilesOfBuilding);
            }

            _temporalRangeTilesToDraw = tilesToDraw;
        }

        private List<TileDataEntity> DrawRangeOfMilitaryBuilding(MilitaryBuildingFacade buildingFacade, Vector3Int position,
            bool canBeCleaned, Tilemap tilemap)
        {
            var temporalRangeTilesToDraw = GetAttackZoneOfBuilding(buildingFacade, position,
                canBeCleaned, tilemap);
            var currentTileAttackArray = CopyFromTileDataToArray(temporalRangeTilesToDraw);
            var tileAttackPositions = GetTilePositionsOfTileData(temporalRangeTilesToDraw);
            SetTilesInTilemap(tileAttackPositions, currentTileAttackArray, tilemap);
            return temporalRangeTilesToDraw;
        }

        private void DrawPositionCenterSavedMilitaryBuildings()
        {
            foreach (var buildingData in _savedBuildings)
            {
                DrawPositionCenterMilitaryBuilding(buildingData.position, _buildingTilemap, TileType.Red);
            }
        }

        private void DrawPositionCenterMilitaryBuilding(Vector3Int buildingDataPosition, Tilemap tilemap,
            TileType tileType)
        {
            _temporalBuildingCenterTileToDraw = GetBuildingZone(tileType, buildingDataPosition);
            var currentTileBuildingArray = CopyFromTileDataToArray(_temporalBuildingCenterTileToDraw);
            var tileBuildingPositions = GetTilePositionsOfTileData(_temporalBuildingCenterTileToDraw);
            SetTilesInTilemap(tileBuildingPositions, currentTileBuildingArray, tilemap);
        }

        private void SetTemporalBuildingZone(MilitaryBuildingFacade militaryBuildingFacade, Vector3Int gridPosition)
        {
            //TODO Encontrar manera de hace esto sin usar BOUNDSINT
            _temporalRangeTilesToDraw = DrawRangeOfMilitaryBuilding(militaryBuildingFacade, gridPosition, true, _weaponToSetRangeTilemap);
            DrawPositionCenterMilitaryBuilding(gridPosition, _buildingTilemap, TileType.Green);
        }

        private Vector3Int[] GetTilePositionsOfTileData(List<TileDataEntity> tileData)
        {
            var positions = tileData.Select(x => x.GridPosition).ToArray();
            return positions;
        }

        private void HideTemporalTileMap()
        {
            _buildingTilemap.gameObject.SetActive(false);
            _weaponToSetRangeTilemap.gameObject.SetActive(false);
            _weaponRangeTilemap.gameObject.SetActive(false);
        }

        private void ShowTemporalTileMap()
        {
            _buildingTilemap.gameObject.SetActive(true);
            _weaponToSetRangeTilemap.gameObject.SetActive(true);
            _weaponRangeTilemap.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!_buildingTilemap.gameObject.activeInHierarchy || !_building) return;
            if (!Input.GetMouseButton(0)) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;

            var buildingGridPosition = GetGridPositionByMouse(Input.mousePosition);
            if (buildingGridPosition == _currentObjectPosition ||
                !PositionIsInsideBuildingTilemapGrid(buildingGridPosition) ||
                _worldTileDictionaryBuildingTilemap[buildingGridPosition].IsOccupied) return;

            ClearPreviousPaintedArea();
            _building.transform.position = _tilemap.GetCellCenterLocal(buildingGridPosition);

            var temporalObjectArea = GetObjectArea(buildingGridPosition, _currentBuildingArea);
            var buildingArray =
                GetTilesBlock(temporalObjectArea, WorldTileDictionary);

            if (buildingArray.Count <= 0) return;
            var buildingCurrentTile = GetColourOfBuildingTiles(buildingArray, _currentBuildingArea);
            _currentObjectPosition = buildingGridPosition;
            var colours = buildingCurrentTile.Select(x => x.TilemapColours[_buildingTilemap].CurrentColour).ToList();
            if (CanBePlacedHere(colours) && _showAttackZone)
            {
                Debug.Log("Can Place");
                _currentBuildingArea = _buildingFacadeComponent.AttackArea;

                SetTemporalBuildingZone(_buildingFacadeComponent, _currentObjectPosition);
                return;
            }

            var buildingArea = GetObjectArea(buildingGridPosition, temporalObjectArea.size);
            SetTilesInTilemap(buildingArea, CopyFromTileDataToArray(buildingCurrentTile), _buildingTilemap);
        }

        private bool PositionIsInsideBuildingTilemapGrid(Vector3Int buildingGridPosition)
        {
            return _worldTileDictionaryBuildingTilemap.ContainsKey(buildingGridPosition) &&
                   _worldTileDictionaryBuildingTilemap[buildingGridPosition].TilemapColours
                       .ContainsKey(_buildingTilemap);
        }

        private List<TileDataEntity> GetBuildingZone(TileType tileTypeBuilding, Vector3Int buildingPosition)
        {
            var tileData = WorldTileDictionary[buildingPosition];
            var tileDataColour = GetTilemapColour(tileData, _buildingTilemap);
            var buildingTileDataEntities = new List<TileDataEntity>();
            tileDataColour.CurrentColour = tileTypeBuilding;
            tileData.GridPosition = buildingPosition;
            tileData.TileBase = _tileTypeBase[tileTypeBuilding];
            tileData.TilemapColours[_buildingTilemap] = tileDataColour;
            buildingTileDataEntities.Add(tileData);
            return buildingTileDataEntities;
        }

        
        private List<TileDataEntity> GetAttackZoneOfBuilding(MilitaryBuildingFacade militaryBuildingFacade,
            Vector3Int buildingPosition, bool canBeCleaned, Tilemap tilemapToDraw)
        {
            var attackArray =
                GetAttackTilesOfBuilding(buildingPosition, militaryBuildingFacade);
            if (attackArray.Any(x => x.GridPosition == buildingPosition))
            {
                var buildingTile = attackArray.Single(x => x.GridPosition == buildingPosition);
                attackArray.Remove(buildingTile);
            }

            return GetColourOfAttackZone(attackArray, canBeCleaned, tilemapToDraw);
        }

        private List<TileDataEntity> GetColourOfAttackZone(List<TileDataEntity> attackArray, bool canBeCleaned,
            Tilemap tilemapToDraw)
        {
            var tileDataToSet = new List<TileDataEntity>();

            for (var i = 0; i < attackArray.Count; i++)
            {
                var colours = attackArray[i].TilemapColours[_weaponToSetRangeTilemap];
                if (attackArray[i].IsOccupied)
                {
                    attackArray[i].TileBase = _tileTypeBase[TileType.Red];
                    colours.CurrentColour = TileType.Red;
                }
                else if (attackArray[i].TilemapColours[_weaponToSetRangeTilemap].CurrentColour == TileType.Red)
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

                attackArray[i].TilemapColours[tilemapToDraw] = colours;

                AddTemporalTileData(attackArray[i], tileDataToSet);
            }

            return tileDataToSet;
        }
        
        private bool CanBePlacedHere(List<TileType> tileArray)
        {
            return tileArray.Any(x => x is TileType.Green or TileType.Blue);
        }

        private List<TileDataEntity> GetColourOfBuildingTiles(List<TileDataEntity> baseArray, Vector3Int buildingArea)
        {
            var tileDatasBuilding = new List<TileDataEntity>();
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

            return tileDatasBuilding;
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
            ClearPaintedTiles(_temporalBuildingCenterTileToDraw, _buildingTilemap);
            ClearAttackPaintedTiles(_temporalRangeTilesToDraw, _weaponToSetRangeTilemap);
            _temporalRangeTilesToDraw.Clear();
            _temporalBuildingCenterTileToDraw.Clear();
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
                    case TileType.Blue when tileMapColour.PreviousColour == TileType.Blue && 
                                            tileMapColour.OriginalColour == TileType.Empty:
                        tile.TileBase = _tileTypeBase[TileType.Empty];
                        tileMapColour.CurrentColour = TileType.Empty;
                        tileMapColour.PreviousColour = TileType.Blue;
                        break;
                    case TileType.Blue when tileMapColour.PreviousColour == TileType.Blue 
                                            && tileMapColour.OriginalColour != TileType.Empty:
                        tile.TileBase = _tileTypeBase[TileType.Blue];
                        tileMapColour.CurrentColour = tileMapColour.PreviousColour;
                        tileMapColour.PreviousColour = tileMapColour.PreviousColour;
                        break;

                    default:
                        tile.TileBase = _tileTypeBase[TileType.Empty];
                        tileMapColour.CurrentColour = TileType.White;
                        tileMapColour.PreviousColour = tileMapColour.PreviousColour;
                        break;
                }

                tilesToClear[index] = tile;
                tilesToClear[index].TilemapColours[tilemap] = tileMapColour;
            }

            var filledTiles = CopyFromTileDataToArray(tilesToClear);
            var tilePositions = GetTilePositionsOfTileData(tilesToClear);

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

        private List<TileDataEntity> GetTilesBlock(BoundsInt area,
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

        private List<TileDataEntity> GetTilesBlockAttacking(BoundsInt area)
        {
            var list = new List<TileDataEntity>();

            foreach (var v in area.allPositionsWithin)
            {
                var pos = new Vector3Int(v.x, v.y, 0);
                if (!WorldTileDictionary.ContainsKey(pos) || list.Contains(WorldTileDictionary[pos])) continue;
                list.Add(WorldTileDictionary[pos]);
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

        public void GetAnyTurretWhichHasTileInRange(TileDataEntity tileDataEntity, out List<MilitaryBuildingFacade> buildingsToReturn)
        {
            buildingsToReturn = new();
            foreach (var building in 
                     _savedBuildings.Where(building => building.buildingFacadeComponent.ContainsTileToAttack(tileDataEntity)))
            {
                buildingsToReturn.Add(building.buildingFacadeComponent);
            }
        } 
    }
}