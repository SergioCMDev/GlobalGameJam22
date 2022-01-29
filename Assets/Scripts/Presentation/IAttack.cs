using UnityEngine;

namespace Presentation
{
    public interface IAttack
    {
        public void Attack(IReceiveDamage objectToAttack);
    }
}