using UnityEngine;

public class GameStatusController : MonoBehaviour
{
    private Enemy _enemy;

    [SerializeField] private PlayerHasWonEvent _playerHasWonEvent;
    [SerializeField] private PlayerHasLostEvent _playerHasLostEvent;

    void Start()
    {
        _enemy = FindObjectOfType<Enemy>();
        _enemy.OnEnemyHasBeenDefeated += EnemyHasBeenDefeated;
        //Escuchar cuando la ciudad esta perdida
    }

    private void EnemyHasBeenDefeated()
    {
        _playerHasWonEvent.Fire();
    }

    private void PlayerHasBeenDefeated()
    {
        _playerHasLostEvent.Fire();
    }
}