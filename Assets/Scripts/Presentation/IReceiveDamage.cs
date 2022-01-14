using UnityEngine;

namespace App.Weapons.Interfaces
{
    public interface IReceiveDamage
    {
        void ReceiveDamage(GameObject itemWhichHit, float receivedDamage);
        void ReceiveDamage(float receivedDamage);
    }
}