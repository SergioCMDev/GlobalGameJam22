using System;
using IA;
using Presentation;
using UnityEngine;
using Utils;

public class Enemy : MonoBehaviour, IReceiveDamage, ILife
{
    [SerializeField] private EnemyMovement _enemyMovement;
    [SerializeField] private int _maximumLife;
    private float _life;

    public event Action OnEnemyHasBeenDefeated;

    private void Start()
    {
        _life = _maximumLife;
    }

    public void ReceiveDamage(float receivedDamage, DamageType damageType)
    {
        //TODO REFRACTOR USING COMMAND PATTERN
        _life -= receivedDamage;
        CheckLife();
        if (damageType != DamageType.TeslaTower) return;
        _enemyMovement.ChangeSpeed(_enemyMovement.Speed *= 0.25f);
        Invoke(nameof(ResetSpeed), 0.4f);

    }

    private void ResetSpeed()
    {
        _enemyMovement.ResetSpeed();
    }

    private void CheckLife()
    {
        if (_life <= 0)
        {
            OnEnemyHasBeenDefeated.Invoke();
        }
    }

    public void AddLife(float lifeToAdd)
    {
        _life = _life.CircularClamp(0, _maximumLife);
    }
}