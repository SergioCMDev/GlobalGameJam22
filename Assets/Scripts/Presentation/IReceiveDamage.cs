using UnityEngine;

namespace Presentation
{
    public interface IReceiveDamage
    {
        void ReceiveDamage(GameObject itemWhichHit, float receivedDamage);
        void ReceiveDamage(float receivedDamage);
    }
}