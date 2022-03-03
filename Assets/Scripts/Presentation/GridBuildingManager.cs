using System;
using System.Collections.Generic;
using System.Linq;
using App.Events;
using Presentation;
using Presentation.Building;
using Presentation.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;


[Serializable]
public struct SetBuildingData
{
    public GameObject building;
    public MilitaryBuildingFacade buildingFacadeComponent;
    public Vector3Int position;
}


[Serializable]
public class TileDataEntity
{
    public TileBase TileBase;
    public Vector3Int GridPosition;
    public bool IsOccupied;
    public GameObject Occupier;
    public TileType OriginalColour;
    public TileType PreviousColour;
    public TileType CurrentColour;
    public Vector3 WorldPosition;
    public bool Locked;

    public TileDataEntity()
    {
        Locked = false;
    }

    public void ResetOccupy()
    {
        IsOccupied = false;
        Occupier = null;
    }
}

public class GridBuildingManager : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap, _buildingTilemap, _weaponRangeTilemap;
    [SerializeField] private Grid _grid;
    [SerializeField] private SaveBuildingEvent saveBuildingEvent;
    [SerializeField] private Tile _red, white, green, _purple;
    [SerializeField] private BuildingHasBeenSetEvent _buildingHasBeenSetEvent;

    private Dictionary<TileType, TileBase> _tileTypeBase = new Dictionary<TileType, TileBase>();

    public IDictionary<Vector3, Vector3Int> world; //REMOVE
    private GameObject _building;
    private Vector3Int _currentObjectPosition, _currentBuildingArea, _originalBuildingArea;

    private MilitaryBuildingFacade _buildingFacadeComponent;

    private readonly List<SetBuildingData> _savedBuildings = new List<SetBuildingData>();
    private readonly List<TileDataEntity> tileDatas = new List<TileDataEntity>();

    private Dictionary<Vector3Int, TileDataEntity> _worldTileDictionary = new Dictionary<Vector3Int, TileDataEntity>();

    public event Action OnPlayerHasCanceledSetBuildingOnGrid;
    public event Action<MilitaryBuildingFacade> OnPlayerHasSetBuildingOnGrid;


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
        var colours = tileDatas.Select(x => x.CurrentColour).ToList();
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
        _worldTileDictionary[_currentObjectPosition].IsOccupied = true;
        _worldTileDictionary[_currentObjectPosition].Occupier = _buildingFacadeComponent.gameObject;
        _buildingFacadeComponent.Deselect();

        var attackTiles = GetAttackTilesOfBuilding(_currentObjectPosition, _buildingFacadeComponent, _buildingTilemap);
        _currentObjectPosition = Vector3Int.zero;
        _buildingFacadeComponent.SetTilesToAttack(attackTiles);
        OnPlayerHasSetBuildingOnGrid?.Invoke(_buildingFacadeComponent);
        _buildingFacadeComponent = null;
        saveBuildingEvent.Instance = _building;
        saveBuildingEvent.Fire();
        tileDatas.Clear();
    }

    private List<TileDataEntity> GetAttackTilesOfBuilding(Vector3Int buildingPosition,
        MilitaryBuildingFacade militaryBuildingFacade, Tilemap tilemap)
    {
        var offset = Vector3Int.up * militaryBuildingFacade.AttackRingRange +
                     Vector3Int.right * militaryBuildingFacade.AttackRingRange;
        var temporalObjectArea = GetObjectArea(buildingPosition - offset, militaryBuildingFacade.AttackArea);
        var attackArray = GetTilesBlock(temporalObjectArea, tilemap);
        return attackArray;
    }


    private void ReadWorld()
    {
        world = new Dictionary<Vector3, Vector3Int>();
        for (int n = _tilemap.cellBounds.xMin; n < _tilemap.cellBounds.xMax; n++)
        {
            for (int p = _tilemap.cellBounds.yMin; p < _tilemap.cellBounds.yMax; p++)
            {
                Vector3Int gridPosition = (new Vector3Int(n, p, (int)_tilemap.transform.position.y));
                Vector3 worldPosition = _tilemap.CellToWorld(gridPosition);
                if (!_tilemap.HasTile(gridPosition) || !_buildingTilemap.HasTile(gridPosition)) continue;

                _worldTileDictionary.Add(gridPosition, new TileDataEntity()
                {
                    WorldPosition = worldPosition,
                    GridPosition = gridPosition,
                    Occupier = null,
                    IsOccupied = false,
                    OriginalColour = GetCurrentTileType(_buildingTilemap.GetTile(gridPosition)),
                    PreviousColour = GetCurrentTileType(_buildingTilemap.GetTile(gridPosition)),
                    CurrentColour = GetCurrentTileType(_buildingTilemap.GetTile(gridPosition)),
                    TileBase = _buildingTilemap.GetTile(gridPosition)
                });
                world.Add(worldPosition, gridPosition);
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
    }

    private void LoadMilitaryBuildings()
    {
        foreach (var buildingData in _savedBuildings)
        {
            SetBuildingRelativeZone(buildingData.buildingFacadeComponent, buildingData.position, TileType.Red, false);
            _worldTileDictionary[buildingData.position].Occupier = buildingData.buildingFacadeComponent.gameObject;
            _worldTileDictionary[buildingData.position].IsOccupied = true;
        }

        tileDatas.Clear();
    }

    private void SetBuildingRelativeZone(MilitaryBuildingFacade militaryBuildingFacade,
        Vector3Int gridPosition,
        TileType buildingColour = TileType.Green, bool canBeCleaned = true)
    {
        SetAttackZone(militaryBuildingFacade, gridPosition, canBeCleaned);
        SetBuildingZone(buildingColour, gridPosition);
        var currentTileArray = CopyFromTileDataToArray();
        var tilePositions = GetTilePositionsOfTileData();
        SetTilesInTilemap(tilePositions, currentTileArray, _buildingTilemap);
    }

    private Vector3Int[] GetTilePositionsOfTileData()
    {
        var positions = tileDatas.Select(x => x.GridPosition).ToArray();
        return positions;
    }

    private void HideTemporalTileMap()
    {
        _buildingTilemap.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!_buildingTilemap.gameObject.activeInHierarchy || !_building) return;
        if (!Input.GetMouseButton(0)) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        var buildingGridPosition = GetGridPositionByMouse(Input.mousePosition);
        if (buildingGridPosition == _currentObjectPosition) return;

        ClearPreviousPaintedArea();
        _building.transform.position = _tilemap.GetCellCenterLocal(buildingGridPosition);

        var temporalObjectArea = GetObjectArea(buildingGridPosition, _currentBuildingArea);
        var buildingArray = GetTilesBlock(temporalObjectArea, _buildingTilemap);

        if (buildingArray.Count <= 0) return;
        SetColourOfBuildingTiles(buildingArray, _currentBuildingArea);
        _currentObjectPosition = buildingGridPosition;
        var colours = tileDatas.Select(x => x.CurrentColour).ToList();

        if (CanBePlacedHere(colours))
        {
            Debug.Log("Can Place");
            _currentBuildingArea = _buildingFacadeComponent.AttackArea;

            SetBuildingRelativeZone(_buildingFacadeComponent, _currentObjectPosition);
            return;
        }

        var buildingArea = GetObjectArea(buildingGridPosition, temporalObjectArea.size);
        SetTilesInTilemap(buildingArea, CopyFromTileDataToArray(), _buildingTilemap);
    }


    private void SetBuildingZone(TileType tileTypeBuilding, Vector3Int buildingPosition)
    {
        var tileData = _worldTileDictionary[buildingPosition];
        tileData.CurrentColour = tileTypeBuilding;
        tileData.GridPosition = buildingPosition;
        tileData.TileBase = _tileTypeBase[tileTypeBuilding];
    }

    private void SetAttackZone(MilitaryBuildingFacade militaryBuildingFacade, Vector3Int buildingPosition,
        bool canBeCleaned)
    {
        tileDatas.Clear();
        switch (militaryBuildingFacade.BuildingAttacker.AttackAreaType)
        {
            case AttackRangeType.Ring:
                var attackArray = GetAttackTilesOfBuilding(buildingPosition, militaryBuildingFacade, _buildingTilemap);
                SetColourOfAttackZone(attackArray, canBeCleaned);

                break;
        }
    }

    private void SetColourOfAttackZone(List<TileDataEntity> attackArray, bool canBeCleaned)
    {
        for (int i = 0; i < attackArray.Count; i++)
        {
            if (attackArray[i].CurrentColour == TileType.Red)
            {
                attackArray[i].TileBase = _tileTypeBase[TileType.Red];
                attackArray[i].CurrentColour = TileType.Red;
                attackArray[i].PreviousColour = TileType.Red;
            }
            else
            {
                attackArray[i].TileBase = _tileTypeBase[TileType.Blue];
                attackArray[i].CurrentColour = TileType.Blue;
                if (!canBeCleaned)
                    attackArray[i].PreviousColour = TileType.Blue;
            }


            AddTemporalTileData(attackArray[i]);
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
            if (baseArray[i].CurrentColour == TileType.White ||
                baseArray[i].CurrentColour == TileType.Blue && baseArray[i].OriginalColour == TileType.White)
            {
                baseArray[i].CurrentColour = TileType.Green;
                baseArray[i].TileBase = _tileTypeBase[TileType.Green];

                AddTemporalTileData(baseArray[i]);
                continue;
            }

            tileDatas.Clear();
            for (var x = 0; i < buildingArea.x * buildingArea.y * buildingArea.z; i++)
            {
                baseArray[x].CurrentColour = TileType.Red;
                baseArray[x].TileBase = _tileTypeBase[TileType.Red];

                AddTemporalTileData(baseArray[x]);
            }

            break;
        }
    }


    private void AddTemporalTileData(TileDataEntity tileDataEntity)
    {
        if (tileDataEntity == null) return;
        tileDatas.Add(tileDataEntity);
    }

    private void ClearPreviousPaintedArea()
    {
        if (tileDatas.Count == 0) return;

        for (var index = 0; index < tileDatas.Count; index++)
        {
            var tile = tileDatas[index];
            switch (tile.CurrentColour)
            {
                case TileType.Green:
                case TileType.Blue when tile.PreviousColour == TileType.White:
                case TileType.Red when tile.PreviousColour == TileType.Empty:
                case TileType.Red when tile.PreviousColour == TileType.Red:
                case TileType.Red when tile.PreviousColour == TileType.White:
                case TileType.Red when tile.PreviousColour == TileType.Blue:
                case TileType.Blue when tile.PreviousColour == TileType.Red:
                case TileType.Blue when tile.PreviousColour == TileType.Blue:
                case TileType.Blue when tile.PreviousColour == TileType.Empty:
                    tile.TileBase = _tileTypeBase[tile.PreviousColour];
                    tile.CurrentColour = tile.PreviousColour;
                    break;
                default:
                    tile.TileBase = _tileTypeBase[TileType.White];
                    tile.CurrentColour = TileType.White;
                    tile.PreviousColour = TileType.White;
                    break;
            }

            tileDatas[index] = tile;
        }

        var filledTiles = CopyFromTileDataToArray();
        var tilePositions = GetTilePositionsOfTileData();

        SetTilesInTilemap(tilePositions, filledTiles, _buildingTilemap);
        tileDatas.Clear();
        _currentBuildingArea = _originalBuildingArea;
    }

    private TileBase[] CopyFromTileDataToArray()
    {
        var filledTiles = new TileBase[tileDatas.Count];
        for (var index = 0; index < tileDatas.Count; index++)
        {
            filledTiles[index] = tileDatas[index].TileBase;
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
        _buildingHasBeenSetEvent.Building = _building;
        _buildingHasBeenSetEvent.buildingFacadeComponent = _buildingFacadeComponent;
        _buildingHasBeenSetEvent.Position = _currentObjectPosition;
        _buildingHasBeenSetEvent.Fire();

        _savedBuildings.Add(new SetBuildingData()
            {
                building = _building,
                buildingFacadeComponent = _buildingFacadeComponent,
                position = _currentObjectPosition
            }
        );
    }

    private List<TileDataEntity> GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        var list = new List<TileDataEntity>();

        foreach (var v in area.allPositionsWithin)
        {
            var pos = new Vector3Int(v.x, v.y, 0);
            if (!_worldTileDictionary.ContainsKey(pos) || list.Contains(_worldTileDictionary[pos])) continue;


            list.Add(_worldTileDictionary[pos]);
        }

        return list;
    }

    public void ObjectHasMovedToNewTile(ObjectHasMovedToNewTileEvent tileEvent)
    {
        _worldTileDictionary[tileEvent.NewPositionToMove].IsOccupied = true;
        _worldTileDictionary[tileEvent.NewPositionToMove].Occupier = tileEvent.occupier;
        _worldTileDictionary[tileEvent.OldPosition].ResetOccupy();
    }
}