using App.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "InstantiateEnemyEvent",
    menuName = "Events/Enemy/InstantiateEnemyEvent")]
public class InstantiateEnemyEvent : GameEventScriptable
{
    public GameObject Prefab;
    public float Life;
    public Vector3Int PositionToInstantiate;
    public float Speed;
}