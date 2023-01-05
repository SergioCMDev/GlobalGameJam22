using System;
using App;
using App.Tuples;
using UnityEngine;

namespace Presentation.Interfaces
{
    public interface IMovable
    {
        public event Action<GameObject, WorldPositionTuple> OnObjectMoved;
    }
}