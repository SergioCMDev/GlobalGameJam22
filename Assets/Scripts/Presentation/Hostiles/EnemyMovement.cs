using App.Events;
using Presentation.Managers;
using UnityEngine;

namespace Presentation.Hostiles
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 0;
        private float _currentSpeed;
        public TilePosition tilePosition;

        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        private void Start()
        {
            _currentSpeed = speed;
        }

        public void ResetSpeed()
        {
            _currentSpeed = speed;
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