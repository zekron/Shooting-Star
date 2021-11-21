using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveRotationAngle = 25f;

    [Header("Move")]
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private AudioDataSO projectileLaunchSFX;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float minFireInterval;
    [SerializeField] private float maxFireInterval;

    private float paddingX;
    private float paddingY;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }

    void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.RandomEnemySpawnPosition(paddingX, paddingY);

        Vector3 targetPosition = Viewport.RandomRightHalfPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            // if has not arrived targetPosition
            if (Vector3.Distance(transform.position, targetPosition) >= moveSpeed * Time.fixedDeltaTime)
            {
                // keep moving to targetPosition
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
                // make enemy rotate with x axis while moving
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }
            else
            {
                // set a new targetPosition
                targetPosition = Viewport.RandomRightHalfPosition(paddingX, paddingY);
            }

            yield return waitForFixedUpdate;
        }
    }

    IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            if (GameManager.GameState == GameState.GameOver) yield break;

            foreach (var projectile in projectiles)
            {
                ObjectPoolManager.Release(projectile, muzzle.position);
            }

            AudioManager.Instance.PlaySFX(projectileLaunchSFX);
        }
    }
}