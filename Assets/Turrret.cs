using Presentation;
using UnityEngine;

public class Turrret : Building
{
    
    
        
    public override void ReceiveDamage(GameObject itemWhichHit, float receivedDamage)
    {
    }

    public override void ReceiveDamage(float receivedDamage)
    {
        Life -= receivedDamage;
        UpdateLifeSliderBar();
    }

    public override void AddLife(float lifeToAdd)
    {
        Life += lifeToAdd;
        UpdateLifeSliderBar();
    }
}
