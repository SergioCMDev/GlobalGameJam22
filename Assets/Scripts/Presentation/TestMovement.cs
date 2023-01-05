using System;
using App;
using App.Events;
using App.Info.Tuples;
using Presentation.Interfaces;
using UnityEngine;

namespace Presentation
{
    public class TestMovement : MonoBehaviour, IMovable
    {
        public event Action<GameObject, WorldPositionTuple> OnObjectMoved;
        private Vector3 _previousObjectWorldPosition;

        private void Start()
        {
            _previousObjectWorldPosition = transform.position;
        }

        void Update()
        {
            if (Math.Abs(_previousObjectWorldPosition.sqrMagnitude - transform.position.sqrMagnitude) < 0.02) return;
            var objectWorldPosition = transform.position;
            OnObjectMoved?.Invoke(gameObject, new WorldPositionTuple()
            {
                NewWorldPosition = objectWorldPosition,
                OldWorldPosition = _previousObjectWorldPosition
            });
            _previousObjectWorldPosition = objectWorldPosition;
        }
    }
}