using System;
using System.Collections.Generic;
using System.Linq;
using Application_;
using Application_.Events;
using Presentation.Building;
using Presentation.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Presentation
{
    [Serializable]
    public struct SetBuildingData
    {
        public GameObject Building;
        public MilitaryBuilding BuildingComponent;
        public Vector3Int Position;
    }

    [Serializable]
    public struct TileData
    {
        public TileBase Tile;
        public TileType PreviousColour;
        public TileType CurrentColour;
        public bool CanBeChanged;
    }

    public class GridBuildingManager : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap, _tilemapOverWorld;
        [SerializeField] private Grid _grid;
        [SerializeField] private SaveBuildingEvent saveBuildingEvent;
        [SerializeField] private Tile _red, white, green, _purple;

        private Dictionary<TileType, TileBase> _tileTypeBase = new Dictionary<TileType, TileBase>();

        public IDictionary<Vector3, Vector3Int> world; //REMOVE
        private GameObject _building;
        private BoundsInt _temporalBuildingArea;
        private Vector3Int _currentPosition;
        private Vector3Int _buildingArea, _originalBuildingArea;

        private MilitaryBuilding _buildingComponent;

        // private TileType _previousColour;
        private readonly List<SetBuildingData> _savedBuildings = new List<SetBuildingData>();
        private List<TileData> tileDatas = new List<TileData>();

        [SerializeField] private BuildingHasBeenSetEvent _buildingHasBeenSetEvent;
        private Vector3Int _attackArea;
        private bool _canBePlacedSomewhere;
        private TileBase[] currentTileArray;

        public event Action OnPlayerHasSetBuildingOnGrid, OnPlayerHasCanceledSetBuildingOnGrid;


        private void Awake()
        {
            string tilePath = @"Tiles\";
            //TODO LOAD FROM RESOURCES
            _tileTypeBase.Add(TileType.Empty, null);
            _tileTypeBase.Add(TileType.White, white);
            // tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
            _tileTypeBase.Add(TileType.Green, green);
            // tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
            // tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
            _tileTypeBase.Add(TileType.Red, _red);
            _tileTypeBase.Add(TileType.Purple, _purple);


            HideTemporalTileMap();
            ReadWorld(); //REMOVE
        }


        public void AllowPlayerToSetBuildingInTilemap(AllowPlayerToSetBuildingInTilemapEvent tilemapEvent)
        {
            ShowTemporalTileMap();
            LoadBuildings();
            _building = Instantiate(tilemapEvent.Prefab); //GET POOL
            _buildingComponent = _building.GetComponent<MilitaryBuilding>();
            _buildingComponent.Initialize();
            _buildingComponent.Select();
            _buildingComponent.SetStatusChooserCanvas(true);
            _buildingComponent.OnCancelTakingPlace += CancelTakingPlace;
            _buildingComponent.OnBuildingTriesToTakePlace += BuildingTriesToTakePlace;
            _buildingArea = _buildingComponent.Area;
            _originalBuildingArea = _buildingComponent.Area;
            _attackArea = _buildingComponent.AttackArea;
            _building.transform.position = _tilemap.GetCellCenterWorld(new Vector3Int());
        }

        private void CancelTakingPlace()
        {
            if (_canBePlacedSomewhere)
            {
                HideAttackZone();
                _canBePlacedSomewhere = false;
            }

            _buildingArea = _originalBuildingArea;
            currentTileArray = new TileBase[] { };

            _buildingComponent.OnCancelTakingPlace -= CancelTakingPlace;
            _buildingComponent.OnBuildingTriesToTakePlace -= BuildingTriesToTakePlace;
            
            HideTemporalTileMap();
            _buildingComponent.Deselect();
            Destroy(_building);
            ClearPreviousPaintedArea();
            
            _buildingComponent = null;
            _currentPosition = Vector3Int.zero;
            OnPlayerHasCanceledSetBuildingOnGrid.Invoke();
        }


        private void BuildingTriesToTakePlace()
        {
            if (!CanTakeArea(_temporalBuildingArea)) return;
            SetBuildingInGrid();
        }

        private void SetBuildingInGrid()
        {
            if (_canBePlacedSomewhere)
            {
                HideAttackZone();
                _canBePlacedSomewhere = false;
            }

            _buildingArea = _originalBuildingArea;
            currentTileArray = new TileBase[] { };
            TakeArea(_temporalBuildingArea);
            _buildingComponent.OnCancelTakingPlace -= CancelTakingPlace;
            _buildingComponent.OnBuildingTriesToTakePlace -= BuildingTriesToTakePlace;
            _buildingComponent.SetStatusChooserCanvas(false);
            var tileArray = CopyFromTileDataToArray();
            var filledTiles = FillTilesWithoutSaving(tileArray, TileType.Red);
            SetTilesInTilemap(_temporalBuildingArea, filledTiles, _tilemapOverWorld);
            HideTemporalTileMap();
            _currentPosition = Vector3Int.zero;
            _buildingComponent.Deselect();

            _buildingComponent = null;
            OnPlayerHasSetBuildingOnGrid.Invoke();
            saveBuildingEvent.Instance = _building;
            saveBuildingEvent.Fire();
            _building = null;
            tileDatas.Clear();
        }


        //REMOVE
        private void ReadWorld()
        {
            world = new Dictionary<Vector3, Vector3Int>();
            for (int n = _tilemap.cellBounds.xMin; n < _tilemap.cellBounds.xMax; n++)
            {
                for (int p = _tilemap.cellBounds.yMin; p < _tilemap.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = (new Vector3Int(n, p, (int)_tilemap.transform.position.y));
                    Vector3 place = _tilemap.CellToWorld(localPlace);
                    if (_tilemap.HasTile(localPlace))
                    {
                        world.Add(place, localPlace);
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

        private void ShowTemporalTileMap()
        {
            _tilemapOverWorld.gameObject.SetActive(true);
        }

        private void LoadBuildings()
        {
            foreach (var buildingData in _savedBuildings)
            {
                var lastArea = GetObjectArea(buildingData.Position, buildingData.BuildingComponent.Area);
                var baseArray = GetTilesBlock(lastArea, _tilemapOverWorld);
                FillTiles(baseArray, TileType.Red);
                for (var index = 0; index < tileDatas.Count; index++)
                {
                    var tile = tileDatas[index];
                    tile.CanBeChanged = false;
                    tileDatas[index] = tile;
                }

                var filledTiles = CopyFromTileDataToArray();
                SetTilesInTilemap(lastArea, filledTiles, _tilemapOverWorld);
            }
        }

        private void HideTemporalTileMap()
        {
            _tilemapOverWorld.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_tilemapOverWorld.gameObject.activeInHierarchy || !_building) return;
            if (!Input.GetMouseButton(0)) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;

            var gridPosition = GetGridPositionByMouse(Input.mousePosition);
            if (gridPosition == _currentPosition) return;

            ClearPreviousPaintedArea();
            _building.transform.position = _tilemap.GetCellCenterLocal(gridPosition);

            _temporalBuildingArea = GetObjectArea(gridPosition, _buildingArea);
            var buildingArray = GetTilesBlock(_temporalBuildingArea, _tilemapOverWorld);
            SetColourOfBuildingTiles(buildingArray, _buildingArea);
            _currentPosition = gridPosition;
            currentTileArray = CopyFromTileDataToArray();
            // if (CanBePlacedHere(currentTileArray))
            // {
            //     _canBePlacedSomewhere = true;
            //     Debug.Log("Can Place");
            //     ShowAttackZone();
            //     return;
            // }

            var buildingArea = GetObjectArea(gridPosition, _temporalBuildingArea.size);
            SetTilesInTilemap(buildingArea, currentTileArray, _tilemapOverWorld);
        }

        private void ShowAttackZone()
        {
            _buildingArea = _attackArea;
            var attackArea = GetObjectArea(_currentPosition, _attackArea);
            var attackArray = GetTilesBlock(attackArea, _tilemapOverWorld);

            tileDatas.Clear();
            FillBuildingArrayWithBuildingSize(attackArray, _temporalBuildingArea.size);
            SetColourOfAttackZone(attackArray, _temporalBuildingArea);
            var filledTiles = CopyFromTileDataToArray();

            SetTilesInTilemap(attackArea, filledTiles, _tilemapOverWorld);
        }

        private void HideAttackZone()
        {
            var temporalAttackArea = GetObjectArea(_currentPosition, _attackArea);
            var tileMixedArray = GetTilesBlock(temporalAttackArea, _tilemapOverWorld);
            for (int i = 0; i < currentTileArray.Length; i++)
            {
                tileMixedArray[i] = currentTileArray[i];
            }

            tileMixedArray = HideColourOfAttackZone(tileMixedArray, _temporalBuildingArea);
            SetTilesInTilemap(temporalAttackArea, tileMixedArray, _tilemapOverWorld);
        }

        private TileBase[] HideColourOfAttackZone(TileBase[] tileMixedArray, BoundsInt buildingArea)
        {
            for (var i = buildingArea.size.x * buildingArea.size.y * buildingArea.size.z;
                 i < tileMixedArray.Length;
                 i++)
            {
                tileMixedArray[i] = _tileTypeBase[TileType.White];
            }

            return tileMixedArray;
        }

        private void SetColourOfAttackZone(TileBase[] tileMixedArray, BoundsInt buildingArea)
        {
            for (var i = buildingArea.size.x * buildingArea.size.y * buildingArea.size.z;
                 i < tileMixedArray.Length;
                 i++)
            {
                AddTileData(_tileTypeBase[TileType.Purple], GetCurrentTileType(tileMixedArray[i]), TileType.Purple);
                tileMixedArray[i] = _tileTypeBase[TileType.Purple];
            }
        }


        private bool CanBePlacedHere(TileBase[] tileArray)
        {
            return tileArray.Any(x => x == _tileTypeBase[TileType.Green]);
        }


        private void SetColourOfBuildingTiles(TileBase[] baseArray, Vector3Int buildingArea)
        {
            for (var i = 0; i < buildingArea.x * buildingArea.y * buildingArea.z; i++)
            {
                if (baseArray[i] == _tileTypeBase[TileType.White])
                {
                    baseArray[i] = _tileTypeBase[TileType.Green];
                    AddTileData(baseArray[i], TileType.White, TileType.Green);
                    continue;
                }

                FillTiles(baseArray, TileType.Red);
                break;
            }
        }

        private void FillBuildingArrayWithBuildingSize(TileBase[] baseArray, Vector3Int buildingArea)
        {
            for (var i = 0; i < buildingArea.x * buildingArea.y * buildingArea.z; i++)
            {
                if (baseArray[i] != _tileTypeBase[TileType.White]) continue;
                AddTileData( _tileTypeBase[TileType.Green], TileType.White, TileType.Green);
                baseArray[i] = _tileTypeBase[TileType.Green];
            }
        }


        private void AddTileData(TileBase tileArray, TileType previousColour, TileType currentColour,
            bool canBeChanged = true)
        {
            if (tileArray == null) return;
            tileDatas.Add(new TileData()
            {
                Tile = tileArray,
                CurrentColour = currentColour,
                PreviousColour = previousColour,
                CanBeChanged = canBeChanged
            });
        }
        
        private void ClearPreviousPaintedArea()
        {
            var lastArea = GetObjectArea(_currentPosition, _buildingArea);
            if (tileDatas.Count == 0) return;

            for (var index = 0; index < tileDatas.Count; index++)
            {
                var tile = tileDatas[index];
                if (!tile.CanBeChanged) continue;
                switch (tile.CurrentColour)
                {
                    case TileType.Green:
                        tile.Tile = _tileTypeBase[tile.PreviousColour];
                        tile.CurrentColour = tile.PreviousColour;
                        break;
                    case TileType.Red when tile.PreviousColour == TileType.Empty:
                        tile.Tile = _tileTypeBase[TileType.Empty];
                        tile.CurrentColour = TileType.Empty;
                        break;
                    case TileType.Red when tile.PreviousColour == TileType.Red:
                        tile.Tile = _tileTypeBase[TileType.Red];
                        tile.CurrentColour = TileType.Red;
                        break;
                    case TileType.Red when tile.PreviousColour == TileType.White:
                        tile.Tile = _tileTypeBase[TileType.White];
                        tile.CurrentColour = TileType.White;
                        break;
                    case TileType.Purple when tile.PreviousColour == TileType.Red:
                        tile.Tile = _tileTypeBase[tile.PreviousColour];
                        tile.CurrentColour = tile.PreviousColour;
                        break;
                    case TileType.Purple when tile.PreviousColour == TileType.White:
                        tile.Tile = _tileTypeBase[tile.PreviousColour];
                        tile.CurrentColour = tile.PreviousColour;
                        break;
                    case TileType.Purple when tile.PreviousColour == TileType.Empty:
                        tile.Tile = _tileTypeBase[TileType.Empty];
                        tile.CurrentColour = TileType.Empty;
                        tile.PreviousColour = TileType.Empty;
                        break;
                    default:
                        tile.Tile = _tileTypeBase[TileType.White];
                        tile.CurrentColour = TileType.White;
                        tile.PreviousColour = TileType.White;
                        break;
                }

                tileDatas[index] = tile;
            }

            var filledTiles = CopyFromTileDataToArray();


            SetTilesInTilemap(lastArea, filledTiles, _tilemapOverWorld);
            tileDatas.Clear();
            _buildingArea = _originalBuildingArea;
        }

        private TileBase[] CopyFromTileDataToArray()
        {
            var filledTiles = new TileBase[tileDatas.Count];
            for (var index = 0; index < tileDatas.Count; index++)
            {
                filledTiles[index] = tileDatas[index].Tile;
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

        private void FillTiles(TileBase[] tileArray, TileType type)
        {
            for (int i = 0; i < tileArray.ToArray().Length; i++)
            {
                AddTileData(_tileTypeBase[type], GetCurrentTileType(tileArray[i]), type);
                tileArray[i] = _tileTypeBase[type];
            }
        }

        private TileBase[] FillTilesWithoutSaving(TileBase[] tileArray, TileType type)
        {
            for (int i = 0; i < tileArray.ToArray().Length; i++)
            {
                tileArray[i] = _tileTypeBase[type];
            }

            return tileArray;
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

            return tile == _tileTypeBase[TileType.Purple] ? TileType.Purple : TileType.White;
        }


        private bool CanTakeArea(BoundsInt area)
        {
            var baseArray = GetTilesBlock(area, _tilemapOverWorld);
            foreach (var tile in baseArray)
            {
                if (tile == _tileTypeBase[TileType.Green]) continue;
                Debug.Log("Cannot place here");
                return false;
            }

            return true;
        }

        private void TakeArea(BoundsInt area)
        {
            SetTilesBlock(area, TileType.Red, _tilemapOverWorld);
            _buildingHasBeenSetEvent.Building = _building;
            _buildingHasBeenSetEvent.BuildingComponent = _buildingComponent;
            _buildingHasBeenSetEvent.Position = _currentPosition;
            _buildingHasBeenSetEvent.Fire();

            _savedBuildings.Add(new SetBuildingData()
                {
                    Building = _building,
                    BuildingComponent = _buildingComponent,
                    Position = _currentPosition
                }
            );
        }

        private TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
        {
            TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
            int counter = 0;

            foreach (var v in area.allPositionsWithin)
            {
                Vector3Int pos = new Vector3Int(v.x, v.y, 0);
                array[counter] = tilemap.GetTile(pos);
                counter++;
            }

            return array;
        }

        private void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
        {
            int size = area.size.x * area.size.y * area.size.z;
            TileBase[] tileArray = new TileBase[size];
            FillTiles(tileArray, type);
            tilemap.SetTilesBlock(area, tileArray);
        }
    }
}