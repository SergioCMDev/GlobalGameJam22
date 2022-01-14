using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public static class UtilsMovements
    {
        public static bool LinearMovements(Vector2 direction)
        {
            return Mathf.Abs(direction.x) == 1 || Mathf.Abs(direction.y) == 1;
        }
        
        public static bool IsOnScreen(Camera camera, Transform transform)
        {
            Vector3 screenPoint = camera.WorldToViewportPoint(transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 &&
                            screenPoint.y < 1;
            return onScreen;
        }
        
        public static Tweener MoveObjectToPositionWithDuration(Transform objectToMove, Transform positionToMove, float duration)
        {
            var positionCubeInfo = objectToMove.transform.position;
            var valueToMoveDownObjectCubeInfo = positionCubeInfo.y - positionToMove.position.y;

            Vector3 destinationPositionObjectSurviveCube =
                positionCubeInfo - Vector3.up * valueToMoveDownObjectCubeInfo;
            var tweenerWs =
                objectToMove.transform.DOMove(destinationPositionObjectSurviveCube, duration,
                    false);
            return tweenerWs;
        }
    }
}