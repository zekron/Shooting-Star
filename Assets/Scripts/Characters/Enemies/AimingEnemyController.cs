using System.Collections;
using UnityEngine;

public class AimingEnemyController : EnemyController
{
    [SerializeField] private Vector3EventChannelSO playerMoveInputEventSO;
    [SerializeField] private Vector3 offset;
    protected override void OnEnable()
    {
        playerMoveInputEventSO.OnEventRaised += SetOffset;
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        playerMoveInputEventSO.OnEventRaised -= SetOffset;
        base.OnDisable();
    }
    private void SetOffset(Vector3 value)
    {
        offset = value;
    }
    protected override void GenerateTargetPosition(float x = float.MaxValue, float y = float.MaxValue)
    {
        targetPosition = (playerTransform.position + offset - transform.position)/*.normalized*/ * 20;
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