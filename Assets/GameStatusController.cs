using Presentation;
using UnityEngine;

public class GameStatusController : MonoBehaviour
{
    private Enemy _enemy;
    [SerializeField] private CityBuilding _cityBuilding;

    [SerializeField] private PlayerHasWonEvent _playerHasWonEvent;
    [SerializeField] private PlayerHasLostEvent _playerHasLostEvent;

    void Start()
    {
        _enemy = FindObjectOfType<Enemy>();
        _enemy.OnEnemyHasBeenDefeated += EnemyHasBeenDefeated;
        _cityBuilding.OnBuildingDestroyed += PlayerHasBeenDefeated;
        //Escuchar cuando la ciudad esta perdida
    }


    private void EnemyHasBeenDefeated()
    {
        _playerHasWonEvent.Fire();
    }

    private void PlayerHasBeenDefeated(Building building)
    {
        _playerHasLostEvent.Fire();
    }
}