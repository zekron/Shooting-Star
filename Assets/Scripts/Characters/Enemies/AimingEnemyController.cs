using System.Collections;
using UnityEngine;

public class AimingEnemyController : EnemyController
{
    protected override void GenerateTargetPosition(float x = float.MaxValue, float y = float.MaxValue)
    {
        //float angle = Vector2.Angle(Vector2.right, playerTransform.position- transform.position);
        //Debug.Log($"{name} {angle}");
        //targetPosition.x = Mathf.Cos(angle) * 50 + transform.position.x;
        //targetPosition.y = Mathf.Cos(angle) * 50 + transform.position.y;
        targetPosition = (playerTransform.position - transform.position).normalized * 20;
    }

    protected override IEnumerator RandomlyMovingCoroutine()
    {
        GenerateTargetPosition();

        while (gameObject.activeSelf)
        {
            if (Vector3.Distance(transform.position, targetPosition) >= moveSpeed * Time.deltaTime)
            {
                character.Move(Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime));
                character.Rotate(Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.up));
            }
            else
            {
                yield break;
            }

            yield return null;
        }
    }
}