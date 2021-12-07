using System.Collections;
using UnityEngine;

public class EnemyProjectile_Aiming : Projectile
{
    void Awake()
    {
    }

    protected override void OnEnable()
    {
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
        StartCoroutine(nameof(MoveDirectionCoroutine));
        base.OnEnable();
    }

    IEnumerator MoveDirectionCoroutine()
    {
        yield return null;

        if (target.activeSelf)
        {
            moveDirection = (target.transform.position - transform.position).normalized;
        }
    }
}