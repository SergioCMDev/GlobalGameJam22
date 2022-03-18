using System.Collections.Generic;
using Presentation.Hostiles;
using UnityEngine;

namespace Presentation.Interfaces
{
    public interface IAttack
    {
        public void Attack(List<Enemy> objectsToAttack);
    }
}