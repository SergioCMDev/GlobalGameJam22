using System;
using System.Collections.Generic;
using System.Linq;
using Application_.Events;
using IA;
using Presentation;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GridPathfinding:MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private bool friend;
    [SerializeField] private Tile cityDestroyed;
    [SerializeField] private CityBuilding _cityBuilding;

    [FormerlySerializedAs("availablePlaces")] public List<Vector3> yellowBrickRoad;
    private List<Vector3> world;
    private List<Vector3> defensiveBuilds;
    private IDictionary<Vector3, Vector3Int> defensiveBuildsToWorld;

    private Vector3 nextDestination;
    private Vector3 lastBrick;
    private bool attacking = false;
    public bool initialPotition = true;
    public float damageTime = 1;

    public Vector3 start;
    public Vector3 end;
    private float currentTime;
    public float damage = 1f;
    public float attackSpeed = 1f;
    
    
    private void Awake()
    {
        defensiveBuildsToWorld = new Dictionary<Vector3, Vector3Int>();
        world = new List<Vector3>();
        start = (friend) ? _tilemap.cellBounds.min : _tilemap.cellBounds.max;
        end = (friend) ? _tilemap.cellBounds.max : _tilemap.cellBounds.min;
        //_enemy.InitialPosition(_tilemap.WorldToLocal(start));
        GetWorld();
        nextDestination = Pathfinder(enemyMovement.transform.position);

        initialPotition = false;
    }

    private void Update()
    {
        if (enemyMovement.transform.position == nextDestination)
        {
            if (attacking)
            {
                //attacking = true;
                //nextDestination = lastBrick;
                if (CanAttack())
                {
                    _cityBuilding.ReceiveDamage(damage);
                    currentTime = Time.deltaTime;
                    _cityBuilding.OnBuildingDestroyed += destroyTile;
                    Debug.Log("Damage percentage: " + (100/_cityBuilding.MaxLife)*_cityBuilding.Life);
                }
            }
            else
            {
                nextDestination = Pathfinder(enemyMovement.transform.position);
            }
        }
        enemyMovement.MoveTo(nextDestination);
    }

    private void destroyTile(Building obj)
    {
        if (((100/_cityBuilding.MaxLife)*_cityBuilding.Life) < 90)
        {
            var randomKey = defensiveBuildsToWorld.Keys.ToArray()[(int)Random.Range(0, defensiveBuildsToWorld.Keys.Count - 1)];
            _tilemap.SetTile(defensiveBuildsToWorld[randomKey], cityDestroyed);
            defensiveBuildsToWorld.Remove(randomKey);
        }
        
    }

    private Vector3 Pathfinder(Vector3 currentPos)
    {
        Vector3 bestOptionVector = new Vector3();
        float bestOption = 99999;
        
        //activar si se quiere atacar a los edificios
        if (!yellowBrickRoad.Any() && !defensiveBuilds.Any())
        //if (!yellowBrickRoad.Any())
        {
            bestOptionVector = end;
        }
        else
        {
            HeuristicValue(currentPos, yellowBrickRoad, ref bestOption,  ref bestOptionVector);
            //activar si se quiere atacar a los edificios
            HeuristicValue(currentPos, defensiveBuilds, ref bestOption, ref bestOptionVector);
        }

        if (defensiveBuilds.Contains(bestOptionVector))
        {
            //_tilemap.GetTile(defensiveBuildsToWorld[bestOptionVector]);
            //Tile aux = Resources.Load<Tile>("Sprites/Bocetoagua");
            
            //defensiveBuilds.Remove(bestOptionVector);
            attacking = true;
            lastBrick = nextDestination;
            
            //Quitar si implementado el ataque a torretas
            bestOptionVector = nextDestination;
            currentTime = Time.deltaTime;
        }
        else if(yellowBrickRoad.Contains(bestOptionVector))
        {
            yellowBrickRoad.Remove(bestOptionVector);
            attacking = false;
        }else if (bestOptionVector == end)
        {
            defensiveBuilds.Clear();
            yellowBrickRoad.Clear();
        }

        return bestOptionVector;
    }

    public void GetWorld()
    {
        yellowBrickRoad = new List<Vector3>();
        defensiveBuilds = new List<Vector3>();
        for (int n = _tilemap.cellBounds.xMin; n < _tilemap.cellBounds.xMax; n++)
        {
            for (int p = _tilemap.cellBounds.yMin; p < _tilemap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int) _tilemap.transform.position.y));
                Vector3 place = _tilemap.CellToWorld(localPlace);
                if (_tilemap.HasTile(localPlace))
                {
                    world.Add(place);
                    if (_tilemap.GetTile(localPlace).name.Equals("bocetoedificos"))
                    {
                        defensiveBuildsToWorld.Add(place, localPlace);
                        defensiveBuilds.Add(place);
                    } 
                    else if (_tilemap.GetTile(localPlace).name.Equals("bocetotierra"))
                    {
                        yellowBrickRoad.Add(place);
                    }
                }
            }
        }
    }

    private void HeuristicValue(Vector3 current, List<Vector3> ListCandidates, ref float result, ref Vector3 chosen)
    {
        float HValue;
        
        foreach(var tile in ListCandidates)
        {
            HValue = Math.Abs(current.x - tile.x) + Math.Abs(current.y - tile.y);
            if (HValue < result)
            {
                chosen = tile;
                result = HValue;
            };
        }
    }
    
    private bool CanAttack()
    {
        return currentTime + attackSpeed > Time.deltaTime;
    }
    
    
    
}
