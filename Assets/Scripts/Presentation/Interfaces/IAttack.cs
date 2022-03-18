using System.Collections.Generic;
using UnityEngine;

namespace Presentation.Interfaces
{
    public interface IAttack
    {
        public void Attack(List<GameObject> objectsToAttack);
    }
}