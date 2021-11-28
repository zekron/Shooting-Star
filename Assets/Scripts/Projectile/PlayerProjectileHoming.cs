using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileHoming : PlayerProjectile
{
    [SerializeField] private float minBallisticAngle = 50f;
    [SerializeField] private float maxBallisticAngle = 75f;

    private float ballisticAngle;

    private Vector3 targetDirection;
    private Quaternion tempRotation;

    protected override void OnEnable()
    {
        SetTarget(GameObject.FindGameObjectWithTag("Enemy"));
        transform.rotation = Quaternion.identity;

        if (target == null)
        {
            base.OnEnable();
        }
        else
        {
            StartCoroutine(HomingCoroutine(target));
        }
    }

    public IEnumerator HomingCoroutine(GameObject target)
    {
        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);

        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {
                targetDirection = target.transform.position - transform.position;
                Debug.DrawLine(target.transform.position, transform.position, Color.red);
                tempRotation = Quaternion.AngleAxis((Mathf.Atan2(targetDirection.y, targetDirection.x) - Mathf.PI / 2) * Mathf.Rad2Deg, Vector3.forward)
                    * Quaternion.Euler(0f, 0f, ballisticAngle);
                Rotate(tempRotation);
            }

            Move(moveDirection * moveSpeed * Time.deltaTime);

            yield return null;
        }
    }
}