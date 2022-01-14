using App.Weapons.Interfaces;
using UnityEngine;

public abstract class LifeSetter : MonoBehaviour, IReceiveDamage, ILife
{
    public abstract void ReceiveDamage(GameObject itemWhichHit, float receivedDamage);

    public abstract void ReceiveDamage(float receivedDamage);

    public abstract void AddLife(float lifeToAdd);
}