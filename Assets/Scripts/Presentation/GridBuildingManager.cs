using System;
using System.Collections.Generic;
using System.Linq;
using Application_;
using Application_.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Presentation
{
    [Serializable]
    public struct TileTuple
    {
        public Vector3 WorlddPosition;
        public Vector3Int GridPosition;
        public TileInnerData TileInnerData;
    }

    public class GridBuildingManager : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap, _tilemapOverWorld;
        [SerializeField] private Grid _grid;

        [SerializeField] private List<TileTuple> innerTileDataFromTiles = new List<TileTuple>();
        [SerializeField] private List<Vector3> tilesToBlock = new List<Vector3>();
        [SerializeField] private SaveBuildingEvent saveBuildingEvent;
        [SerializeField] private Tile _red, white, green;

        private Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

        public IDictionary<Vector3, Vector3Int> world;
        private GameObject _building;
        private BoundsInt _temporalArea;
        private Vector3Int _currentPosition;
        private MilitaryBuilding _buildingComponent;
        private Vector3Int _buildingArea;
        private TileType previousColour;

        private void Awake()
        {
            string tilePath = @"Tiles\";
            //TODO LOAD FROM RESOURCES
            tileBases.Add(TileType.Empty, null);
            tileBases.Add(TileType.White, white);
            // tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
            tileBases.Add(TileType.Green, green);
            // tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
            // tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
            tileBases.Add(TileType.Red, _red);


            _tilemapOverWorld.gameObject.SetActive(false);
            ReadWorld();
            foreach (var tilePosition in world.Keys)
            {
                innerTileDataFromTiles.Add(new TileTuple()
                {
                    GridPosition = world[tilePosition],
                    WorlddPosition = tilePosition,
                    TileInnerData = new TileInnerData()
                });
            }

            foreach (var tileToBlock in tilesToBlock)
            {
                var gridPosition = world[tileToBlock];
                if (innerTileDataFromTiles.All(x => x.GridPosition != gridPosition)) continue;
                innerTileDataFromTiles.Single(x => x.GridPosition != gridPosition).TileInnerData.CanBeUsed = false;
            }
        }


        public void PlayerSetBuildingInTilemap(PlayerSetBuildingInTilemapEvent tilemapEvent)
        {
            ShowTemporalTileMap();
            _building = Instantiate(tilemapEvent.Prefab); //GET POOL
            _buildingComponent = _building.GetComponent<MilitaryBuilding>();
            _buildingComponent.SetStatusChooserCanvas(true);
            _buildingComponent.OnCancelTakingPlace += CancelTakingPlace;
            _buildingComponent.OnBuildingTriesToTakePlace += BuildingTriesToTakePlace;
            _buildingArea = _buildingComponent.Area;

            _building.transform.position = _tilemap.GetCellCenterWorld(new Vector3Int());
            saveBuildingEvent.Instance = _building;
            saveBuildingEvent.Fire();
        }

        private void CancelTakingPlace()
        {
            _buildingComponent.OnCancelTakingPlace -= CancelTakingPlace;
            _buildingComponent.OnBuildingTriesToTakePlace -= BuildingTriesToTakePlace;
            HideTemporalTileMap();
            _buildingComponent = null;
            Destroy(_building);
        }


        private void BuildingTriesToTakePlace()
        {
            if (!CanTakeArea(_temporalArea)) return;
            TakeArea(_temporalArea);
            _buildingComponent.OnCancelTakingPlace -= CancelTakingPlace;
            _buildingComponent.OnBuildingTriesToTakePlace -= BuildingTriesToTakePlace;
            _buildingComponent.SetStatusChooserCanvas(false);
            HideTemporalTileMap();
            _building = null;
            _buildingComponent = null;
        }


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


        public Vector3Int GetGridPositionByMouse(Vector3 inputMousePosition)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(inputMousePosition);
            Vector3Int gridPosition = _grid.LocalToCell(mousePosition);
            return gridPosition;
        }


        // private TileInnerData GetTileData(Vector3Int gridPosition)
        // {
        //     return innerTileDataFromTiles.Single(x => x.GridPosition == gridPosition)
        //         .TileInnerData;
        // }


        // public bool PositionExists(Vector3 gridPosition)
        // {
        //     return innerTileDataFromTiles.Any(x => x.GridPosition == gridPosition);
        // }

        // public bool IsOccupied(Vector3 gridPosition)
        // {
        //     return innerTileDataFromTiles.Single(x => x.GridPosition == gridPosition)
        //         .TileInnerData.Occupied;
        // }


        // public bool CanBeUsed(Vector3Int gridPosition)
        // {
        //     return GetTileData(gridPosition).CanBeUsed;
        // }

        public void ShowTemporalTileMap()
        {
            _tilemapOverWorld.gameObject.SetActive(true);
        }

        private void HideTemporalTileMap()
        {
            // ClearPreviousPaintedArea();
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
            _currentPosition = gridPosition;
            _building.transform.position = _tilemap.GetCellCenterLocal(_currentPosition);
            _temporalArea = GetBuildingArea(_currentPosition, _buildingArea);
            var baseArray = GetTilesBlock(_temporalArea, _tilemapOverWorld);
            var tileArray = SetColourOfBuildingTiles(baseArray);
            var buildingArea = GetBuildingArea(_currentPosition, _temporalArea.size);
            SetTilesInTilemap(buildingArea, tileArray, _tilemapOverWorld);
        }

        private TileBase[] SetColourOfBuildingTiles(TileBase[] baseArray)
        {
            var size = baseArray.Length;
            var tileArray = new TileBase[size];
            for (var i = 0; i < baseArray.Length; i++)
            {
                if (baseArray[i] == tileBases[TileType.White])
                {
                    tileArray[i] = tileBases[TileType.Green];
                }
                else
                {
                    tileArray = FillTiles(tileArray, TileType.Red);
                    break;
                }
            }

            return tileArray;
        }





        //TODO CHECK THIS RIGHT TO SAVE RED TILES TOO AFTER SET A BUILDING
        private void ClearPreviousPaintedArea()
        {
            var lastArea = GetBuildingArea(_currentPosition, _buildingArea);
            var baseArray = GetTilesBlock(lastArea, _tilemapOverWorld);
            var filledTiles = FillTiles(baseArray, TileType.White);
            SetTilesInTilemap(lastArea, filledTiles, _tilemapOverWorld);
        }

        private BoundsInt GetBuildingArea(Vector3Int gridPosition, Vector3Int sizeArea)
        {
            BoundsInt buildingArea = new BoundsInt(gridPosition, sizeArea);
            return buildingArea;
        }

        private void SetTilesInTilemap(BoundsInt buildingArea, TileBase[] tileArray, Tilemap tilemap)
        {
            tilemap.SetTilesBlock(buildingArea, tileArray);
        }

        private TileBase[] FillTiles(TileBase[] tileArray, TileType type)
        {
            for (int i = 0; i < tileArray.ToArray().Length; i++)
            {
                tileArray[i] = tileBases[type];
            }

            return tileArray;
        }


        private bool CanTakeArea(BoundsInt area)
        {
            var baseArray = GetTilesBlock(area, _tilemapOverWorld);
            foreach (var tile in baseArray)
            {
                if (tile == tileBases[TileType.Green]) continue;
                Debug.Log("Cannot place here");
                return false;
            }

            return true;
        }

        private void TakeArea(BoundsInt area)
        {
            SetTilesBlock(area, TileType.Red, _tilemapOverWorld);
            _tilemapOverWorld.RefreshAllTiles();
            // SetTilesBlock(area, TileType.Green, _tilemap);
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