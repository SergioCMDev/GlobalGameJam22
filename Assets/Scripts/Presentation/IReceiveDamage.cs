using UnityEngine;

namespace Presentation
{
    public interface IReceiveDamage
    {
        void ReceiveDamage(float receivedDamage, DamageType damageType);
    }
}