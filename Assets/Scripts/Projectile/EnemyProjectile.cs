using UnityEngine;

public class EnemyProjectile : Projectile
{
    void Awake()
    {
        if (moveDirection != Vector2.down)
        {
            transform.rotation = Quaternion.FromToRotation(Vector2.down, moveDirection);
        }
    }
}