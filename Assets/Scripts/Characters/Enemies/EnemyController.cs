using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveRotationAngle = 25f;

    private Character character;

    private float paddingX;
    private float paddingY;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    void Awake()
    {
        character = GetComponent<Character>();

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }

    void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.RandomEnemySpawnPosition(paddingX, paddingY);

        Vector3 targetPosition = Viewport.RandomTopHalfPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            // if has not arrived targetPosition
            if (Vector3.Distance(transform.position, targetPosition) >= moveSpeed * Time.fixedDeltaTime)
            {
                // keep moving to targetPosition
                character.Move(Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
                // make enemy rotate with x axis while moving
                character.Rotate(Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.up));

            }
            else
            {
                // set a new targetPosition
                targetPosition = Viewport.RandomTopHalfPosition(paddingX, paddingY);
            }

            yield return null;
        }
    }
}