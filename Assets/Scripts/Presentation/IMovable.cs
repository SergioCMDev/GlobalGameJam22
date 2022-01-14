using UnityEngine;


public interface IMovable
{
    Vector3 VectorOfMovement { get; }
    void SetMovementSpeed(float movementSpeed);
    void StopMovement();
    void ResumeMovement();
}