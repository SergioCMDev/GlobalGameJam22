using UnityEngine;

namespace Utils
{
    public class Vector3Utils
    {
        public static float GetRotationOfDirection(Transform objectTransform, Vector3 vectorOfMovement)
        {
            float difference = Vector3.SignedAngle(objectTransform.forward, vectorOfMovement, Vector3.up);
            var cross = Vector3.Cross(objectTransform.forward, vectorOfMovement);
            return GetSenseOfRotation(cross) * difference;
        }

        private static int GetSenseOfRotation(Vector3 vector3)
        {
            return (vector3.y < 0 ? (-1) : 1);
        }

        //For normalized vectors Dot returns 1 if they point in exactly the same direction, -1 if they point in completely opposite directions and zero if the vectors are perpendicular.
        public static bool VectorsAreInOppositeDirection(Vector3 vectorOfMovement, Vector3 vector3)
        {
            return Vector3.Dot(vectorOfMovement.normalized, vector3.normalized) < 0;
        }

        public static bool Vector3Distance(Vector3 transformPosition, Vector3 position, float distanceToCheck)
        {
            return Vector3.Distance(transformPosition, position) < distanceToCheck;
        }

        public static bool ObjectIsBehindOtherObject(Transform objectWhichHit, Transform transform)
        {
            var forwardHitObject = objectWhichHit.transform.forward;
            var forwardOriginalObject = transform.forward;
            float dot = Vector3.Dot(forwardHitObject, forwardOriginalObject);
            Vector3 cross = Vector3.Cross(forwardHitObject, forwardOriginalObject);
            float difference = Vector3.SignedAngle(objectWhichHit.forward, forwardOriginalObject, Vector3.up);
            Debug.Log(difference);
            if (difference != 0)
            {
                return false;
            }
             
            bool isBehind = dot > 0 && cross.y >= 0 * (difference < 0 ? -1 : 1);
            return isBehind;
        }
    }
}