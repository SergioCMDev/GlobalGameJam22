using System;
using System.Collections.Generic;
using System.Linq;
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
        public GameObject building;
        public MilitaryBuilding buildingComponent;
        public Vector3Int position;
    }

    [Serializable]
    public struct TileData
    {
        public TileBase tile;
        public TileType previousColour;
        public TileType currentColour;
        public bool canBeChanged;
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
        private BoundsInt _temporalObjectArea;
        private Vector3Int _currentObjectPosition, _currentBuildingArea, _originalBuildingArea;

        private MilitaryBuilding _buildingComponent;

        private readonly List<SetBuildingData> _savedBuildings = new List<SetBuildingData>();
        private readonly List<TileData> tileDatas = new List<TileData>();

        [SerializeField] private BuildingHasBeenSetEvent _buildingHasBeenSetEvent;
        private TileBase[] _currentTileArray;

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
            _tileTypeBase.Add(TileType.Blue, _purple);


            HideTemporalTileMap();
            ReadWorld(); //REMOVE
        }


        public void AllowPlayerToSetBuildingInTilemap(AllowPlayerToSetBuildingInTilemapEvent tilemapEvent)
        {
            ShowTemporalTileMap();
            LoadMilitaryBuildings();
            _building = Instantiate(tilemapEvent.Prefab); //GET POOL
            _buildingComponent = _building.GetComponent<MilitaryBuilding>();
            _buildingComponent.Initialize();
            _buildingComponent.Select();
            _buildingComponent.SetStatusChooserCanvas(true);
            _buildingComponent.OnCancelTakingPlace += CancelTakingPlace;
            _buildingComponent.OnBuildingTriesToTakePlace += BuildingTriesToTakePlace;
            _currentBuildingArea = _buildingComponent.Area;
            _originalBuildingArea = _buildingComponent.Area;
            _building.transform.position = _tilemap.GetCellCenterWorld(new Vector3Int());
        }

        private void CancelTakingPlace()
        {
            _buildingComponent.OnCancelTakingPlace -= CancelTakingPlace;
            _buildingComponent.OnBuildingTriesToTakePlace -= BuildingTriesToTakePlace;

            HideTemporalTileMap();
            _buildingComponent.Deselect();
            Destroy(_building);
            ClearPreviousPaintedArea();

            OnPlayerHasCanceledSetBuildingOnGrid.Invoke();
        }


        private void BuildingTriesToTakePlace()
        {
            if (!CanBePlacedHere(_currentTileArray)) return;
            SetBuildingInGrid();
        }

        private void SetBuildingInGrid()
        {
            SaveBuilding();
            //TODO REMOVE THE ATTACK ZONE TO LEAVE WHITE ZONE SURROUNDING THE BUILDING
            _buildingComponent.OnCancelTakingPlace -= CancelTakingPlace;
            _buildingComponent.OnBuildingTriesToTakePlace -= BuildingTriesToTakePlace;
            _buildingComponent.SetStatusChooserCanvas(false);
            ClearPreviousPaintedArea();
            // HideTemporalTileMap();
            _currentObjectPosition = Vector3Int.zero;
            _buildingComponent.Deselect();
            _buildingComponent = null;
            OnPlayerHasSetBuildingOnGrid.Invoke();
            saveBuildingEvent.Instance = _building;
            saveBuildingEvent.Fire();
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

        private void LoadMilitaryBuildings()
        {
            foreach (var buildingData in _savedBuildings)
            {
                SetAttackZone(buildingData.buildingComponent, buildingData.position,
                    buildingData.buildingComponent.AttackArea, false);
                SetBuildingZone(TileType.Red, buildingData.buildingComponent.AttackArea);

                var filledTiles = CopyFromTileDataToArray();

                SetTilesInTilemap(_temporalObjectArea, filledTiles, _tilemapOverWorld);
            }

            tileDatas.Clear();
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
            if (gridPosition == _currentObjectPosition) return;

            ClearPreviousPaintedArea();
            _building.transform.position = _tilemap.GetCellCenterLocal(gridPosition);

            _temporalObjectArea = GetObjectArea(gridPosition, _currentBuildingArea);
            var buildingArray = GetTilesBlock(_temporalObjectArea, _tilemapOverWorld);
            SetColourOfBuildingTiles(buildingArray, _currentBuildingArea);
            _currentObjectPosition = gridPosition;
            _currentTileArray = CopyFromTileDataToArray();
            if (CanBePlacedHere(_currentTileArray))
            {
                Debug.Log("Can Place");
                _currentBuildingArea = _buildingComponent.AttackArea;

                SetAttackZone(_buildingComponent, _currentObjectPosition, _currentBuildingArea);
                SetBuildingZone(TileType.Green, _currentBuildingArea);
                _currentTileArray = CopyFromTileDataToArray();

                SetTilesInTilemap(_temporalObjectArea, _currentTileArray, _tilemapOverWorld);
                return;
            }

            var buildingArea = GetObjectArea(gridPosition, _temporalObjectArea.size);
            SetTilesInTilemap(buildingArea, _currentTileArray, _tilemapOverWorld);
        }


        private void SetBuildingZone(TileType tileTypeBuilding, Vector3Int buildingArea)
        {
            int GetPositionOfBuilding()
            {
                return buildingArea.x * buildingArea.y / 2;
            }

            int positionOfBuilding = GetPositionOfBuilding();
            tileDatas[positionOfBuilding] = new TileData()
            {
                tile = _tileTypeBase[tileTypeBuilding],
                currentColour = tileTypeBuilding,
                previousColour = tileDatas[positionOfBuilding].previousColour,
                canBeChanged = tileDatas[positionOfBuilding].canBeChanged
            };
        }

        private void SetAttackZone(MilitaryBuilding militaryBuilding, Vector3Int buildingPosition,
            Vector3Int attackArea, bool canBeChanged = true)
        {
            var offset = Vector3Int.up * militaryBuilding.AttackRingRange +
                         Vector3Int.right * militaryBuilding.AttackRingRange;
            tileDatas.Clear();

            switch (militaryBuilding.AttackAreaType)
            {
                case AttackRangeType.Ring:
                    _temporalObjectArea = GetObjectArea(buildingPosition - offset, attackArea);
                    TileBase[] attackArray = GetTilesBlock(_temporalObjectArea, _tilemapOverWorld);
                    SetColourOfAttackZone(attackArray, militaryBuilding.AttackArea, canBeChanged);

                    break;
            }
        }

        private void SetColourOfAttackZone(TileBase[] attackArray, Vector3Int attackArea, bool canBeChanged = true)
        {
            for (int i = 0; i < attackArea.x * attackArea.y * attackArea.z; i++)
            {
                TileBase currentTile;
                TileType currentColour, previousColour;
                
                if (GetCurrentTileType(attackArray[i]) == TileType.Red)
                {
                    currentTile = _tileTypeBase[TileType.Red];
                    currentColour = TileType.Red;
                    previousColour = TileType.Red;
                }
                else
                {
                    currentTile = _tileTypeBase[TileType.Blue];
                    currentColour = TileType.Blue;
                    previousColour = GetCurrentTileType(attackArray[i]) == TileType.Blue
                        ? TileType.Blue
                        : GetCurrentTileType(attackArray[i]) == TileType.Empty
                            ? TileType.Empty
                            : TileType.White;
                }

                AddTileData(currentTile, previousColour, currentColour, canBeChanged);
            }
        }

        private bool CanBePlacedHere(TileBase[] tileArray)
        {
            return tileArray.Any(x => x == _tileTypeBase[TileType.Green] || x == _tileTypeBase[TileType.Blue]);
        }

        private void SetColourOfBuildingTiles(TileBase[] baseArray, Vector3Int buildingArea)
        {
            for (var i = 0; i < buildingArea.x * buildingArea.y * buildingArea.z; i++)
            {
                if (baseArray[i] == _tileTypeBase[TileType.White] || baseArray[i] == _tileTypeBase[TileType.Blue])
                {
                    AddTileData(_tileTypeBase[TileType.Green], GetCurrentTileType(baseArray[i]), TileType.Green);
                    baseArray[i] = _tileTypeBase[TileType.Green];
                    continue;
                }

                FillTiles(baseArray, TileType.Red);
                break;
            }
        }

        private void AddTileData(TileBase tileArray, TileType previousColour, TileType currentColour,
            bool canBeChanged = true)
        {
            if (tileArray == null) return;
            tileDatas.Add(new TileData()
            {
                tile = tileArray,
                currentColour = currentColour,
                previousColour = previousColour,
                canBeChanged = canBeChanged
            });
        }

        private void ClearPreviousPaintedArea()
        {
            if (tileDatas.Count == 0) return;

            for (var index = 0; index < tileDatas.Count; index++)
            {
                var tile = tileDatas[index];
                if (!tile.canBeChanged) continue;
                switch (tile.currentColour)
                {
                    case TileType.Green:
                    case TileType.Blue when tile.previousColour == TileType.White:
                    case TileType.Red when tile.previousColour == TileType.Empty:
                    case TileType.Red when tile.previousColour == TileType.Red:
                    case TileType.Red when tile.previousColour == TileType.White:
                    case TileType.Red when tile.previousColour == TileType.Blue:
                    case TileType.Blue when tile.previousColour == TileType.Red:
                    case TileType.Blue when tile.previousColour == TileType.Blue:
                    case TileType.Blue when tile.previousColour == TileType.Empty:
                        tile.tile = _tileTypeBase[tile.previousColour];
                        tile.currentColour = tile.previousColour;
                        break;
                    default:
                        tile.tile = _tileTypeBase[TileType.White];
                        tile.currentColour = TileType.White;
                        tile.previousColour = TileType.White;
                        break;
                }

                tileDatas[index] = tile;
            }

            var filledTiles = CopyFromTileDataToArray();


            SetTilesInTilemap(_temporalObjectArea, filledTiles, _tilemapOverWorld);
            tileDatas.Clear();
            _currentBuildingArea = _originalBuildingArea;
        }

        private TileBase[] CopyFromTileDataToArray()
        {
            var filledTiles = new TileBase[tileDatas.Count];
            for (var index = 0; index < tileDatas.Count; index++)
            {
                filledTiles[index] = tileDatas[index].tile;
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
            _buildingHasBeenSetEvent.Building = _building;
            _buildingHasBeenSetEvent.BuildingComponent = _buildingComponent;
            _buildingHasBeenSetEvent.Position = _currentObjectPosition;
            _buildingHasBeenSetEvent.Fire();

            _savedBuildings.Add(new SetBuildingData()
                {
                    building = _building,
                    buildingComponent = _buildingComponent,
                    position = _currentObjectPosition
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
    }
}