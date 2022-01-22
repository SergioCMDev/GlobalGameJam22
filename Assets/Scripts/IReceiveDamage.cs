using UnityEngine;

namespace Presentation.Interfaces
{
    public interface IReceiveDamage
    {
        void ReceiveDamage(GameObject itemWhichHit, float receivedDamage);
        void ReceiveDamage(float receivedDamage);
    }
}