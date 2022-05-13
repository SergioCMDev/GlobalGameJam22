using App.Events;
using UnityEngine;

namespace Presentation.Hostiles
{
    public class EnemyMovement : MonoBehaviour
    {
        private float _speed;
        private float _currentSpeed;
        public TilePosition tilePosition;

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        private void Start()
        {
            _currentSpeed = _speed;
        }

        public void ResetSpeed()
        {
            _currentSpeed = _speed;
        }

        public void ChangeSpeed(float newSpeed)
        {
            _currentSpeed = newSpeed;
        }
        
        public void MoveTo(Vector3 position)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, _currentSpeed * Time.deltaTime);
        }
        
        public void Stop()
        {
            _currentSpeed = 0;
        }

        public void StopEnemyMovement(StopMovementEnemyEvent levelEvent)
        {
            Stop();
        }

        public void ResumeEnemyMovement(ResumeMovementEnemyEvent levelEvent)
        {
            ResetSpeed();
        }
    }
}