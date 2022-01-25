using System.Collections.Generic;
using UnityEngine;

/*
 * Generate the Grid
 * We can set the dimensions in the Editor.
 */
public class GridCreator : MonoBehaviour
{
    [SerializeField] private int _gridHorizontalSize, _gridVerticalSize;
    [SerializeField] private GameObject _tilePrefab;

    private List<GridTile> _gridTuples = new List<GridTile>();
    private Sprite _tileSprite;
    private float _tileSize;

    public int HorizontalSize => _gridHorizontalSize;
    public int VerticalSize => _gridVerticalSize;

    private void Awake()
    {
        _tileSize = 1;
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        for (int row = 0; row < _gridVerticalSize; row++)
        {
            for (int column = 0; column < _gridHorizontalSize; column++)
            {
                var positionToInstantiate = GetPositionToInstantiate(row, column);

                var tileInstance = Instantiate(_tilePrefab, positionToInstantiate, Quaternion.identity, transform);

                var tileEntity = tileInstance.GetComponent<TileEntity>();
                tileEntity.Walkable = true;

                var gridTuple = new GridTile()
                {
                    tileEntity = tileEntity,
                    tuplePosition = positionToInstantiate
                };

                _gridTuples.Add(gridTuple);
            }
        }
    }

    private Vector3 GetPositionToInstantiate(int row, int column)
    {
        var position = new Vector3(row * _tileSize, column * _tileSize, 1);

        return position;
    }

    public List<GridTile> GetGrid()
    {
        return _gridTuples;
    }
}