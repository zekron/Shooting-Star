using UnityEngine;

public interface IMoveable
{
    void Move(Vector3 moveDirection);
    void Rotate(Quaternion moveRotation);
}