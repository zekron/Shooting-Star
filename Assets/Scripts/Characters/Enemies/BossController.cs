using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    [SerializeField] private bool canRandomlyMove = true;
    [SerializeField] private Vector2 defaultPosition;
    protected override IEnumerator RandomlyMovingCoroutine()
    {
        if (!canRandomlyMove)
        {
            targetPosition = defaultPosition;
            while (gameObject.activeSelf)
            {
                if (Vector3.Distance(transform.position, targetPosition) >= moveSpeed * Time.deltaTime)
                {
                    character.Move(Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime));
                }
                else
                {
                    if (!playerTransform) TryGetPlayerTransform();

                    targetPosition.x = playerTransform.position.x;
                }
                yield return null;
            }
        }
        else
        {
            base.RandomlyMovingCoroutine();
        }
    }
    public override void StopChasingPlayer()
    {
        base.StopChasingPlayer();

        //targetPosition = defaultPosition;
    }
}