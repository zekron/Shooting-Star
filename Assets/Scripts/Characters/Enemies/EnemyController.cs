using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected float moveSpeed = 2f;
    private float moveRotationAngle = 25f;

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

    private void OnEnable()
    {
        TryGetPlayerTransform();
        transform.position = Viewport.RandomEnemySpawnPosition(paddingX, paddingY);

        StartCoroutine(nameof(RandomlyMovingCoroutine));
    }

    protected void TryGetPlayerTransform()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    protected virtual IEnumerator RandomlyMovingCoroutine()
    {
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
    public virtual void StopChasingPlayer()
    {
        StopCoroutine(nameof(ChasingPlayerCoroutine));
    }

    protected IEnumerator ChasingPlayerCoroutine()
    {
        while (isActiveAndEnabled)
        {
            if (!playerTransform) TryGetPlayerTransform();

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