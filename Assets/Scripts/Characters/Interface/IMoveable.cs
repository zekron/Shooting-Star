using UnityEngine;

public interface IMoveable
{
    void Move(Vector2 moveDirection);
    void Rotate(Quaternion moveRotation);
}