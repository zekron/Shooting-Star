using System.Collections;
using UnityEngine;

public class PlayerBomb : PlayerMiniNuke
{
   protected override void PhysicsOverlapDetection()
    {
        // Enemies within explosion radius take AOE damage
        var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayerMask);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.GetDamage(explosionDamage);
            }
            else if (collider.TryGetComponent<EnemyProjectile>(out EnemyProjectile enemyProjectile))
            {
                enemyProjectile.gameObject.SetActive(false);
            }
        }
    }
}