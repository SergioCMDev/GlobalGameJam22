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
        private Vector3Int _lastPosition;
        private Vector3Int _buildingArea, _originalBuildingArea;
        private MilitaryBuilding _buildingComponent;
        private TileType _previousColour;
        private readonly List<SetBuildingData> _savedBuildings = new List<SetBuildingData>();

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
            _building = Instantiate(tilemapEvent.Prefab); //GET POOL
            _buildingComponent = _building.GetComponent<MilitaryBuilding>();
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
            _buildingComponent = null;
            _currentPosition = Vector3Int.zero;
            Destroy(_building);
            switch (_previousColour)
            {
                //TODO GET PREVIOUS ZONE AND SET COLOUR
                case TileType.Green:
                case TileType.Red:
                case TileType.White:
                    _previousColour = TileType.White;
                    break;
                default:
                    _previousColour = TileType.Empty;
                    break;
            }

            ClearPreviousPaintedArea();
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
            HideTemporalTileMap();
            _currentPosition = Vector3Int.zero;
            _buildingComponent = null;
            _previousColour = TileType.Red;
            OnPlayerHasSetBuildingOnGrid.Invoke();
            saveBuildingEvent.Instance = _building;
            saveBuildingEvent.Fire();
            _building = null;
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


        public void ShowTemporalTileMap()
        {
            _tilemapOverWorld.gameObject.SetActive(true);
            LoadBuildings();
        }

        private void LoadBuildings()
        {
            foreach (var buildingData in _savedBuildings)
            {
                var lastArea = GetObjectArea(buildingData.Position, buildingData.BuildingComponent.Area);
                var baseArray = GetTilesBlock(lastArea, _tilemapOverWorld);
                var filledTiles = FillTiles(baseArray, TileType.Red);
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
            if (gridPosition == _lastPosition) return;

            ClearPreviousPaintedArea();
            _currentPosition = gridPosition;
            _lastPosition = _currentPosition;
            _building.transform.position = _tilemap.GetCellCenterLocal(_currentPosition);

            _temporalBuildingArea = GetObjectArea(_currentPosition, _buildingArea);
            var baseArray = GetTilesBlock(_temporalBuildingArea, _tilemapOverWorld);
            currentTileArray = SetColourOfBuildingTiles(baseArray);
            if (CanBePlacedHere(currentTileArray))
            {
                _canBePlacedSomewhere = true;
                Debug.Log("Can Place");
                ShowAttackZone(currentTileArray);
                return;
            }

            var buildingArea = GetObjectArea(_currentPosition, _temporalBuildingArea.size);
            SetTilesInTilemap(buildingArea, currentTileArray, _tilemapOverWorld);
        }

        private void ShowAttackZone(TileBase[] tileArray)
        {
            _buildingArea = _attackArea;
            //Añadir tiles de ataque a tileArray
            var temporalAttackArea = GetObjectArea(_currentPosition, _attackArea);
            var tileMixedArray = GetTilesBlock(temporalAttackArea, _tilemapOverWorld);
            for (int i = 0; i < tileArray.Length; i++)
            {
                tileMixedArray[i] = tileArray[i];
            }

            tileMixedArray = SetColourOfAttackZone(tileMixedArray, _temporalBuildingArea);
            //Fill tiles de rango de ataque
            SetTilesInTilemap(temporalAttackArea, tileMixedArray, _tilemapOverWorld);
        }

        private void HideAttackZone()
        {
            // _buildingArea = _attackArea;
            //Añadir tiles de ataque a tileArray
            var temporalAttackArea = GetObjectArea(_currentPosition, _attackArea);
            var tileMixedArray = GetTilesBlock(temporalAttackArea, _tilemapOverWorld);
            for (int i = 0; i < currentTileArray.Length; i++)
            {
                tileMixedArray[i] = currentTileArray[i];
            }

            tileMixedArray = HideColourOfAttackZone(tileMixedArray, _temporalBuildingArea);
            //Fill tiles de rango de ataque
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

        private TileBase[] SetColourOfAttackZone(TileBase[] tileMixedArray, BoundsInt buildingArea)
        {
            for (var i = buildingArea.size.x * buildingArea.size.y * buildingArea.size.z;
                i < tileMixedArray.Length;
                i++)
            {
                tileMixedArray[i] = _tileTypeBase[TileType.Purple];
            }

            return tileMixedArray;
        }

        private bool CanBePlacedHere(TileBase[] tileArray)
        {
            return tileArray.Any(x => x == _tileTypeBase[TileType.Green]);
        }

        private TileBase[] SetColourOfBuildingTiles(TileBase[] baseArray)
        {
            var size = baseArray.Length;
            var tileArray = new TileBase[size];
            for (var i = 0; i < baseArray.Length; i++)
            {
                if (baseArray[i] == _tileTypeBase[TileType.White])
                {
                    tileArray[i] = _tileTypeBase[TileType.Green];
                    _previousColour = TileType.White;
                    continue;
                }

                if (baseArray[i] == _tileTypeBase[TileType.Green])
                {
                    tileArray = FillTiles(tileArray, TileType.Green);
                    _previousColour = TileType.Mixed;

                    break;
                }

                _previousColour = baseArray[i] == _tileTypeBase[TileType.Empty] ? TileType.Empty : TileType.Red;

                tileArray = FillTiles(tileArray, TileType.Red);
                break;
            }

            return tileArray;
        }


        private void ClearPreviousPaintedArea()
        {
            var lastArea = GetObjectArea(_lastPosition, _buildingArea);
            var baseArray = GetTilesBlock(lastArea, _tilemapOverWorld);
            if (_previousColour == TileType.Mixed || _previousColour == TileType.Purple)
            {
                _previousColour = TileType.White;
            }

            var filledTiles = FillTiles(baseArray,
                _previousColour == TileType.Empty ? TileType.Empty : _previousColour);

            SetTilesInTilemap(lastArea, filledTiles, _tilemapOverWorld);
            _buildingArea = _originalBuildingArea;
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

        private TileBase[] FillTiles(TileBase[] tileArray, TileType type)
        {
            for (int i = 0; i < tileArray.ToArray().Length; i++)
            {
                tileArray[i] = _tileTypeBase[type];
            }

            return tileArray;
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