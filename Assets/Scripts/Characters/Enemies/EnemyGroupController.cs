using System.Collections;
using UnityEngine;

public class EnemyGroupController : EnemyController
{
    [SerializeField] private float waitForNextEnemyMovementInterval = 0.2f;
    [SerializeField, Range(0, 2)] float radius = 1;
    /// <summary>
    /// 预设路径
    /// </summary>
    [SerializeField] private Vector3[] railPositions;
    [SerializeField] private VoidEventChannelSO enemyDestroyEventSO;

    /// <summary>
    /// 每个Enemy在当前移动的路径终点
    /// </summary>
    private Vector3[] targetPositions;
    private Enemy[] enemys;
    private WaitForSeconds waitForNextEnemyMovement;
    private int aliveAmount = 0;
    /// <summary>
    /// 圆心
    /// </summary>
    private Vector3 circle;

    protected override void Awake()
    {
        enemys = GetComponentsInChildren<Enemy>();
        targetPositions = new Vector3[enemys.Length];
        railPositions = new Vector3[36];
        waitForNextEnemyMovement = new WaitForSeconds(waitForNextEnemyMovementInterval);

        var size = enemys[0].transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x;
        paddingY = size.y;
    }

    protected override void OnEnable()
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].transform.localPosition = Vector3.zero;
            enemys[i].gameObject.SetActive(true);
            aliveAmount++;
        }

        transform.position = Viewport.RandomEnemySpawnPosition(paddingX, paddingY);
        circle = Viewport.RandomTopHalfPosition(paddingX, paddingY);
        GenerateRail();
        StartCoroutine(nameof(RandomlyMovingCoroutine));
    }

    protected override void OnDisable()
    {
        aliveAmount = 0;
        base.OnDisable();
        enemyDestroyEventSO.RaiseEvent();
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < railPositions.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(railPositions[i], 0.05f);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(circle, 0.05f);
    }

    private void GenerateRail()
    {
        var start = transform.position;

        //斜边
        var distance = Vector3.Distance(circle, start);
        //切线长度
        var length = Mathf.Sqrt(distance * distance - radius * radius);
        var angle = Mathf.Acos(length / distance);

        //求切点（两个）
        var pointOfTangency = MyMath.LinePoint.RotateAroundPoint(circle, start, angle);
        railPositions[0] = pointOfTangency;

        int endPositionIndex = Random.Range(10, 30);
        for (int i = 1; i <= endPositionIndex; i++)
        {
            railPositions[i] = i != endPositionIndex ?
                MyMath.LinePoint.RotateAroundPoint(pointOfTangency, circle, Mathf.Deg2Rad * -10 * i)   //顺时针要用 -10 * i
                : MyMath.LinePoint.GetPointInLineWithX(
                    railPositions[endPositionIndex - 1],
                    MyMath.LinePoint.RotateAroundPoint(circle, railPositions[i - 1],
                    Mathf.Deg2Rad * 90), Viewport.MaxX + 2);
        }
    }

    private void GetNextTargetPosition(int enemyIndex, ref int currentIndex)
    {
        currentIndex = currentIndex + 1 >= railPositions.Length ? 0 : currentIndex + 1;
        targetPositions[enemyIndex] = railPositions[currentIndex];
    }

    protected override IEnumerator RandomlyMovingCoroutine()
    {

        for (int i = 0; i < enemys.Length; i++)
        {
            targetPositions[i] = railPositions[0];
            StartCoroutine(RandomlyMovingCoroutineSingle(i, 0));
            yield return waitForNextEnemyMovement;
        }
    }

    private IEnumerator RandomlyMovingCoroutineSingle(int enemyIndex, int posIndex)
    {
        var temp = targetPositions[enemyIndex];
        Enemy enemy = enemys[enemyIndex];

        while (enemy.gameObject.activeSelf)
        {
            if (Vector3.Distance(enemy.transform.position, temp) >= enemy.MoveSpeed * Time.deltaTime)
            {
                enemy.Move(Vector2.MoveTowards(enemy.transform.position, temp, enemy.MoveSpeed * Time.deltaTime));
                enemy.Rotate(Quaternion.AngleAxis((temp - enemy.transform.position).normalized.y * enemy.MoveRotationAngle, Vector3.up));
            }
            else
            {
                GetNextTargetPosition(enemyIndex, ref posIndex);
                temp = targetPositions[enemyIndex];
            }

            yield return null;
        }
        aliveAmount--;
        if (aliveAmount <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}