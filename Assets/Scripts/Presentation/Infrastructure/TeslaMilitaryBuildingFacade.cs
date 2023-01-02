using System.Collections.Generic;
using System.Linq;
using Presentation.Hostiles;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public class TeslaMilitaryBuildingFacade : MilitaryBuildingFacade
    {
        private readonly List<Enemy> _enemies = new();
        
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
            if(!hasAttackOnce) return;
            if (tilesToAttack.Any(x => x.Occupier == occupier))
            {
                // Debug.Log("TESLA ENEMY SIGUE DENTRO");
                return;
            }
            hasAttackOnce = false;
            // Debug.Log("TESLA ENEMY SALE");
            RemoveEnemy(occupier.GetComponent<Enemy>());
        }
    }
}