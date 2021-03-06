using UnityEngine;

public class PlayerProjectile : Projectile
{
    TrailRenderer trail;

    protected virtual void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();

        if (moveDirection != Vector2.up)
        {
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.up, moveDirection);
        }
    }

    protected override void OnDisable()
    {
        trail?.Clear();
        base.OnDisable();
    }

    //protected override void OnCollisionEnter2D(Collision2D collision)
    //{
    //    base.OnCollisionEnter2D(collision);
    //    PlayerEnergy.Instance.GainEnergy(PlayerEnergy.PERCENT);
    //}
}