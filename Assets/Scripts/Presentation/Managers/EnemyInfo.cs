using UnityEngine;

namespace Presentation.Managers
{
    [CreateAssetMenu(fileName = "EnemyInfo", menuName = "Enemy/Info")]
    public class EnemyInfo : ScriptableObject
    {
        public float movementSpeed;
        public float attackSpeed;
        public float life;
        public float damage;
    }
}