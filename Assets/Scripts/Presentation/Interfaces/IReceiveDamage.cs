namespace Presentation.Interfaces
{
    public interface IReceiveDamage
    {
       void ReceiveDamage(float receivedDamage);
       void UpdateLifeToMaximum(float maxLife);
    }
}