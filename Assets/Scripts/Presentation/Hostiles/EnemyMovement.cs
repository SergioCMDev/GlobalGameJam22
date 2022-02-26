using Application_.Events;
using UnityEngine;

namespace Presentation.Hostiles
{
    public class EnemyMovement:MonoBehaviour
    {
        [SerializeField] private float mSpeed = 0;
        private float _currentSpeed;
        public float Speed
        {
            get => mSpeed;
            set => mSpeed = value;
        }

        private void Start()
        {
            _currentSpeed = mSpeed;
        }

        public void ResetSpeed()
        {
            _currentSpeed = mSpeed;
        }
        
        public void ChangeSpeed(float newSpeed)
        {
            _currentSpeed = newSpeed;
        }
        

        public void MoveTo(Vector3 position)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, _currentSpeed * Time.deltaTime);
        }

        public void InitialPosition(Vector3 initial)
        {
            transform.position = initial;
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