using UnityEngine;

namespace Utils
{
    public class VectorUtils
    {
        public static float GetRotationOfDirection(Transform objectTransform, Vector3 vectorOfMovement,
            bool senseInverted = true)
        {
            float difference = Vector3.SignedAngle(objectTransform.up, vectorOfMovement, Vector3.up);
            var dot = Vector3.Cross(objectTransform.up, vectorOfMovement);
            if (senseInverted)
            {
                return GetSenseOfRotationInverted(dot) * difference;
            }

            return GetSenseOfRotation(dot) * difference;
        }

        private static int GetSenseOfRotation(Vector3 vector3)
        {
            return (vector3.z > 0 ? (-1) : 1);
        }

        private static int GetSenseOfRotationInverted(Vector3 vector3)
        {
            return (vector3.z < 0 ? (-1) : 1);
        }

        //For normalized vectors Dot returns 1 if they point in exactly the same direction, -1 if they point in completely opposite directions and zero if the vectors are perpendicular.
        public static bool VectorsAreInOppositeDirection(Vector3 vectorOfMovement, Vector3 vector3)
        {
            return Vector3.Dot(vectorOfMovement.normalized, vector3.normalized) < 0;
        }

        public static bool VectorIsNearVector(Vector3 transformPosition, Vector3 position,
            float distanceToCheck)
        {
            return Vector3.Distance(transformPosition, position) < distanceToCheck;
        }
        
        public static bool Vector3IsNearVector3(Vector3 transformPosition, Vector3 position,
            float distanceToCheck)
        {
            return Vector3.Distance(transformPosition, position) < distanceToCheck;
        }

        public static Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal, Transform transform)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
        }

        public static float GetAngleFromVector(Vector3 direction)
        {
            direction = direction.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            return angle;
        }
        
        
        public static Quaternion GetTargetRotation(Transform characterTransform, Vector3 positionToLook)
        {
            var positionVector = positionToLook - characterTransform.position;

// Match the new y value to the object's Y value.
// This ensures that the rotation is calculated only with the X and Z
// I would love to know why this is happening... but I didn't find anything in my initial research
            // positionVector.x = characterTransform.position.x;
            // positionVector.y = characterTransform.position.y;
            // positionVector.z = characterTransform.position.z;
            // positionVector.z = characterTransform.position.z;

// Now we calculate the rotation
            var targetRotation = Quaternion.LookRotation(positionVector, characterTransform.up);
// FYI, if your object's final rotation is off by 90 degrees, you can do the following
// I think it has to do with what the system thinks "forward" is, and which way your model is facing by default.
// So you can either fix it in your model, or add/subtract 90 degrees
// Note that **multiplying** Quaternions together effectively **combines** them
            return targetRotation;
        }
    }
}