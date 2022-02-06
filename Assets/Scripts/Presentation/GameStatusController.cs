using Application_.Events;
using Application_.SceneManagement;
using Presentation;
using UnityEngine;
using Utils;

public class GameStatusController : MonoBehaviour
{
    private Enemy _enemy;
    [SerializeField] private CityBuilding _cityBuilding;

    [SerializeField] private PlayerHasWonEvent _playerHasWonEvent;
    [SerializeField] private PlayerHasLostEvent _playerHasLostEvent;
    private SceneChanger _sceneChanger;

    void Start()
    {
        _sceneChanger = ServiceLocator.Instance.GetService<SceneChanger>();
        _enemy = FindObjectOfType<Enemy>();
        if (_enemy)
            _enemy.OnEnemyHasBeenDefeated += EnemyHasBeenDefeated;
        _cityBuilding.OnBuildingDestroyed += PlayerHasBeenDefeated;
    }


    private void EnemyHasBeenDefeated()
    {
        _playerHasWonEvent.Fire();
    }

    private void PlayerHasBeenDefeated(Building building)
    {
        _playerHasLostEvent.Fire();
    }

    public void PlayerHasWon(PlayerHasWonEvent levelEvent)
    {
        _sceneChanger.GoToNextScene();
    }

    public void PlayerHasLost(PlayerHasLostEvent levelEvent)
    {
        PlayerHasBeenDefeated(null);
    }

    public void RestartLevel(PlayerHasRestartedLevelEvent levelEvent)
    {
        _sceneChanger.RestartScene(levelEvent);
    }
}