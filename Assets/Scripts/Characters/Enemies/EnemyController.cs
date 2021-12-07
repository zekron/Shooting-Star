using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float moveSpeed = 2f;
    private float moveRotationAngle = 25f;

    private Character character;

    private Transform playerTransform;
    protected Vector3 targetPosition;
    protected float paddingX;
    protected float paddingY;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    protected virtual void Awake()
    {
        character = GetComponent<Character>();

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }

    private void OnEnable()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(nameof(RandomlyMovingCoroutine));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.RandomEnemySpawnPosition(paddingX, paddingY);

        targetPosition = Viewport.RandomTopHalfPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            if (Vector3.Distance(transform.position, targetPosition) >= moveSpeed * Time.deltaTime)
            {
                character.Move(Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime));
                character.Rotate(Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.up));
            }
            else
            {
                targetPosition = Viewport.RandomTopHalfPosition(paddingX, paddingY);
            }

            yield return null;
        }
    }
    public void StartChasingPlayer()
    {
        StartCoroutine(nameof(ChasingPlayerCoroutine));
    }
    public void StopChasingPlayer()
    {
        StopCoroutine(nameof(ChasingPlayerCoroutine));
    }
    private IEnumerator ChasingPlayerCoroutine()
    {
        while (isActiveAndEnabled)
        {
            targetPosition.x = playerTransform.position.x;
            targetPosition.y = Viewport.MaxY - paddingY;

            yield return null;
        }
    }

    public void SetMoveProfile(float moveSpeed, float moveRotationAngle)
    {
        this.moveSpeed = moveSpeed;
        this.moveRotationAngle = moveRotationAngle;
    }
}