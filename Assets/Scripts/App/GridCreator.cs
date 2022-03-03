using System.Collections.Generic;
using UnityEngine;

/*
 * Generate the Grid
 * We can set the dimensions in the Editor.
 */
namespace App
{
    public class GridCreator : MonoBehaviour
    {
        [SerializeField] private int _gridHorizontalSize, _gridVerticalSize;
        [SerializeField] private GameObject _tilePrefab;

        private List<GridTile> _gridTuples = new List<GridTile>();
        private Sprite _tileSprite;
        [SerializeField] private float _tileSizeHorizontal, _tileSizeVertical;

        public int HorizontalSize => _gridHorizontalSize;
        public int VerticalSize => _gridVerticalSize;

        private void Awake()
        {
            // _tileSizeHorizontal = 1;
            GenerateGrid();
        }

        public void GenerateGrid()
        {
            for (int row = 0; row < _gridVerticalSize; row++)
            {
                for (int column = 0; column < _gridHorizontalSize; column++)
                {
                    var positionToInstantiate = GetPositionToInstantiate(column, row);

                    var tileInstance = Instantiate(_tilePrefab, positionToInstantiate, Quaternion.identity, transform);
                    tileInstance.name = $"Tile {row} + {column}";
                    var tileEntity = tileInstance.GetComponent<TileEntity>();
                    var spriteRenderer = tileInstance.GetComponent<SpriteRenderer>();
                    spriteRenderer.sortingOrder = _gridVerticalSize - row ;
                    tileEntity.Walkable = true;

                    var gridTuple = new GridTile()
                    {
                        tileEntity = tileEntity,
                        tuplePosition = positionToInstantiate
                    };

                    _gridTuples.Add(gridTuple);
                }

                //GenerateLayerBetween();
            }
        }

        private Vector3 GetPositionToInstantiate(int column, int row)
        {
            var position = new Vector3(column * _tileSizeHorizontal * (column * 2)/4, row *  _tileSizeVertical * (row * 2)/4, 1);

            return position;
        }

        public List<GridTile> GetGrid()
        {
            return _gridTuples;
        }
    }
}