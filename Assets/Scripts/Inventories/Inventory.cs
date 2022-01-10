using System.Collections;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour, IMoveable, IRotate
{
    [Header("Basic Inventory")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private AudioDataSO dropInventorySFX;

    protected float paddingX;
    protected float paddingY;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    protected virtual void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
    }

    protected virtual IEnumerator RandomlyMovingCoroutine()
    {
        //transform.position = Viewport.RandomEnemySpawnPosition(paddingX, paddingY);

        Vector3 targetPosition = Viewport.RandomPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            // if has not arrived targetPosition
            if (Vector3.Distance(transform.position, targetPosition) >= MoveSpeed * Time.deltaTime)
            {
                // keep moving to targetPosition
                Move(Vector2.MoveTowards(transform.position, targetPosition, MoveSpeed * Time.deltaTime));
                Rotate(Quaternion.AngleAxis(Time.realtimeSinceStartup * 100 % 360, Vector3.up));
            }
            else
            {
                // set a new targetPosition
                targetPosition = Viewport.RandomPosition(paddingX, paddingY);
            }

            yield return null;
        }
    }

    public void Move(Vector2 moveDirection)
    {
        transform.position = moveDirection;
    }

    public void Rotate(Quaternion moveRotation)
    {
        transform.rotation = moveRotation;
    }
    public void Drop(Vector3 position)
    {
        ObjectPoolManager.Release(gameObject, position);
        AudioManager.Instance.PlaySFX(dropInventorySFX);
    }
}