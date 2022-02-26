using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Presentation.Utils
{
    public class WallCollisionChecker : MonoBehaviour
    {
        [SerializeField] private float _rayLenght;
        [SerializeField] private string _tagToCheck = "Wall";
        private NativeArray<RaycastCommand> _raycastCommands;
        private NativeArray<RaycastHit> _raycastHits;
        private JobHandle _jobHandle;
        public Action OnWallAhead;
        public Action OnNotWall;
        private Vector3 _direction = Vector3.forward;

        public Vector3 Direction
        {
            get => _direction;
            set => _direction = value;
        }
    
        void Awake()
        {
            _raycastCommands = new NativeArray<RaycastCommand>(1, Allocator.Persistent);
            _raycastHits = new NativeArray<RaycastHit>(1, Allocator.Persistent);
        }

        void Update()
        {
            // 1. Process raycast from last frame
            _jobHandle.Complete();
            RaycastHit raycastHit = _raycastHits[0];
            bool didHitSomething = raycastHit.collider != null;
            if (didHitSomething && raycastHit.collider.CompareTag(_tagToCheck))
            {
                Debug.Log("Wall Ahead");
                OnWallAhead.Invoke();
            }
            else
            {
                Debug.Log("Path Clear");
                OnNotWall.Invoke();
            }

            // 2. Schedule new raycast
            _raycastCommands[0] = new RaycastCommand(transform.position, (Direction * _rayLenght).normalized, _rayLenght);
            _jobHandle = RaycastCommand.ScheduleBatch(_raycastCommands, _raycastHits, 1);
            Debug.DrawRay(transform.position, Direction * _rayLenght, Color.yellow);
        }

        private void OnDestroy()
        {
            try
            {
                _jobHandle.Complete();
                _raycastCommands.Dispose();
                _raycastHits.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void OnDisable()
        {
            try
            {
                _jobHandle.Complete();
                _raycastCommands.Dispose();
                _raycastHits.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, Direction * _rayLenght);
        }
    }
}