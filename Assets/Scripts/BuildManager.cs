using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildManager: MonoBehaviour
{
    [SerializeField] private Tilemap map;
    [SerializeField] private TileBase tile;
    [SerializeField] private MapManager _mapManager;
    
    [SerializeField] public List<Build> city;

    public void setBuildOnGround()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int tilemapPos = map.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Tile tile = map.GetTile<Tile>(tilemapPos);

            Debug.Log("Posicion: (" + tilemapPos.x + ", " + tilemapPos.y + ", " + tilemapPos.z + ")");
        }
    }

}
