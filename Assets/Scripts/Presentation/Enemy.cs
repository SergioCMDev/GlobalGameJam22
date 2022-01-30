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
    private bool _isAlive;

    public event Action OnEnemyHasBeenDefeated;

    private void Start()
    {
        _life = _maximumLife;
    }

    public void ReceiveDamage(float receivedDamage, DamageType damageType)
    {
        //TODO REFRACTOR USING COMMAND PATTERN
        _life -= receivedDamage;
        CheckIfAlive();
        if (damageType != DamageType.TeslaTower) return;
        _enemyMovement.ChangeSpeed(_enemyMovement.Speed *= 0.25f);
        Invoke(nameof(ResetSpeed), 0.4f);

    }

    public bool IsAlive()
    {
        return _life > 0;
    }

    private void ResetSpeed()
    {
        _enemyMovement.ResetSpeed();
    }

    private void CheckIfAlive()
    {
        if (IsAlive()) return;
        _enemyMovement.Stop();
        OnEnemyHasBeenDefeated.Invoke();
    }

    public void AddLife(float lifeToAdd)
    {
        _life = _life.CircularClamp(0, _maximumLife);
    }
}