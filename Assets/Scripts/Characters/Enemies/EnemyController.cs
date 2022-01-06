using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected float moveSpeed = 2f;
    protected float moveRotationAngle = 25f;

    protected Character character;

    protected Transform playerTransform;
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

    protected virtual void OnEnable()
    {
        TryGetPlayerTransform();
        transform.position = Viewport.RandomEnemySpawnPosition(paddingX, paddingY);

        StartCoroutine(nameof(RandomlyMovingCoroutine));
    }

    protected void TryGetPlayerTransform()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void GenerateTargetPosition(float x = float.MaxValue, float y = float.MaxValue)
    {
        if (x == float.MaxValue && y == float.MaxValue)
            targetPosition = Viewport.RandomTopHalfPosition(paddingX, paddingY);
        else
        {
            targetPosition.x = Mathf.Clamp(x, Viewport.MinX + paddingX, Viewport.MaxX - paddingX);
            targetPosition.y = Mathf.Clamp(y, Viewport.MinY + paddingY, Viewport.MaxY - paddingY);
        }
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }

    protected virtual IEnumerator RandomlyMovingCoroutine()
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
                GenerateTargetPosition();
            }

            yield return null;
        }
    }
    public void StartChasingPlayer()
    {
        StartCoroutine(nameof(ChasingPlayerCoroutine));
    }
    public virtual void StopChasingPlayer()
    {
        StopCoroutine(nameof(ChasingPlayerCoroutine));
    }

    protected IEnumerator ChasingPlayerCoroutine()
    {
        while (isActiveAndEnabled)
        {
            if (!playerTransform) TryGetPlayerTransform();

            GenerateTargetPosition(playerTransform.position.x, Viewport.MaxY - paddingY);

            yield return null;
        }
    }

    public void SetMoveProfile(float moveSpeed, float moveRotationAngle)
    {
        this.moveSpeed = moveSpeed;
        this.moveRotationAngle = moveRotationAngle;
    }
}