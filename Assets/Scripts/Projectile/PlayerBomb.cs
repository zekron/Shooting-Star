using System;
using System.Collections;
using UnityEngine;

public class PlayerBomb : PlayerMiniNuke
{
    [SerializeField] private GameObject explosionVFX = null;
    [SerializeField] private AudioDataSO explosionSFX = null;

    protected override void OnEnable()
    {
        StartCoroutine(nameof(DelayDetonation));
        base.OnEnable();
    }

    private IEnumerator DelayDetonation()
    {
        yield return new WaitForSeconds(0.5f);

        // Spawn a explosion VFX
        ObjectPoolManager.Release(explosionVFX, transform.position);
        // Play explosion SFX
        AudioManager.Instance.PlaySFX(explosionSFX);

        PhysicsOverlapDetection();
        gameObject.SetActive(false);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        StopCoroutine(nameof(DelayDetonation));

        // Spawn a explosion VFX
        ObjectPoolManager.Release(explosionVFX, transform.position);
        // Play explosion SFX
        AudioManager.Instance.PlaySFX(explosionSFX);

        PhysicsOverlapDetection();
        gameObject.SetActive(false);
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