using System.Collections;
using UnityEngine;

public class PlayerBomb : PlayerMiniNuke
{
    [SerializeField] private GameObject explosionVFX = null;
    [SerializeField] private AudioDataSO explosionSFX = null;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Spawn a explosion VFX
        ObjectPoolManager.Release(explosionVFX, transform.position);
        // Play explosion SFX
        AudioManager.Instance.PlaySFX(explosionSFX);
        base.OnCollisionEnter2D(collision);
    }
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