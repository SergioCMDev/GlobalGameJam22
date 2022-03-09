using UnityEngine;

namespace Presentation.Building
{
    public abstract class AttackBehaviour : ScriptableObject
    {
        public abstract void Init(GameObject objectToAttack, AttackBehaviourData attackBehaviourData);

        public abstract void DoAttack();
    }
}