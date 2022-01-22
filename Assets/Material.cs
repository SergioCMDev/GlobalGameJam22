using Presentation;
using UnityEngine;

public abstract class Material : MonoBehaviour, IReceiveDamage
{
    public abstract void ReceiveDamage(GameObject itemWhichHit, float receivedDamage);

    public abstract void ReceiveDamage(float receivedDamage);
}