using System;
using System.Collections;
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
        switch (damageType)
        {
            case DamageType.Bullets:
                _life -= receivedDamage;
                CheckLife();
                break;
            case DamageType.TeslaTower:
                _life -= receivedDamage;
                CheckLife();
                _enemyMovement.ChangeSpeed(_enemyMovement.Speed *= 0.25f);
                Invoke(nameof(ResetSpeed), 0.4f);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(damageType), damageType, null);
        }
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