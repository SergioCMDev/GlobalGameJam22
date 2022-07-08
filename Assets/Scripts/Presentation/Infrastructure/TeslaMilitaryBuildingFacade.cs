using System.Collections.Generic;
using System.Linq;
using Presentation.Hostiles;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public class TeslaMilitaryBuildingFacade : MilitaryBuildingFacade
    {
        private List<Enemy> _enemies;
        private bool _hasAttackOnce;

        protected virtual void Update()
        {
            if (!_isActive || !_militaryBuildingAttacker.CanAttack()) return;
            var enemiesToAttack = GetReachableEnemies();
            if (!enemiesToAttack.Any()) return;

            _hasAttackOnce = true;
            SaveEnemiesToAttack(enemiesToAttack);
            _militaryBuildingAttacker.Attack(enemiesToAttack);
        }

        protected override void SaveEnemiesToAttack(List<Enemy> enemiesToAttack)
        {
            foreach (var enemy in enemiesToAttack.Where(enemy => !_enemies.Contains(enemy)))
            {
                _enemies.Add(enemy);
            }
        }

        private void RemoveEnemy(Enemy enemy)
        {
            if (!_enemies.Contains(enemy)) return;
            _enemies.Remove(enemy);

            enemy.ResetSpeed();
        }

        public void CheckIfEnemyIsOutside(GameObject occupier)
        {
            if(!_hasAttackOnce) return;
            if (tilesToAttack.Any(x => x.Occupier == occupier))
            {
                Debug.Log("ENEMY SIGUE DENTRO");
                return;
            }

            Debug.Log("ENEMY SALE");
            RemoveEnemy(occupier.GetComponent<Enemy>());
        }
    }
}