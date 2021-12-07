using System.Collections;
using UnityEngine;

public class PlayerMiniNuke : PlayerProjectile
{
    [Header("==== SPEED CHANGE ====")]
    [SerializeField] private float lowSpeed = 8f;
    [SerializeField] private float highSpeed = 25f;
    [SerializeField] private float variableSpeedDelay = 0.5f;

    [Header("==== EXPLOSION ====")]
    [SerializeField] private GameObject explosionVFX = null;
    [SerializeField] private AudioDataSO explosionSFX = null;
    [SerializeField] protected LayerMask enemyLayerMask = default;
    [SerializeField] protected float explosionRadius = 3f;
    [SerializeField] protected float explosionDamage = 100f;

    private WaitForSeconds waitVariableSpeedDelay;

    protected override void Awake()
    {
        base.Awake();
        waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedCoroutine));
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        // Spawn a explosion VFX
        ObjectPoolManager.Release(explosionVFX, transform.position);
        // Play explosion SFX
        AudioManager.Instance.PlaySFX(explosionSFX);
        PhysicsOverlapDetection();
    }

    IEnumerator VariableSpeedCoroutine()
    {
        MoveSpeed = lowSpeed;

        yield return waitVariableSpeedDelay;

        MoveSpeed = highSpeed;
    }
    protected virtual void PhysicsOverlapDetection()
    {
        // Enemies within explosion radius take AOE damage
        var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayerMask);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.GetDamage(explosionDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}