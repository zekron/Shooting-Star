using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputSO input;

    [Header("Regeneration")]
    [SerializeField] bool regenerateHealth = true;
    [SerializeField] float healthRegenerateTime;
    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;

    [Header("Move")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float accelerationTime = 3f;
    [SerializeField] private float decelerationTime = 3f;
    [SerializeField] private float moveRotationAngle = 50f;

    [Header("Fire")]
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private GameObject projectileOverdrive;
    [SerializeField] private Transform[] muzzles;
    //[SerializeField] AudioData projectileLaunchSFX;
    [SerializeField, Range(0, 2)] private int weaponPower = 0;
    [SerializeField] private float fireInterval = 0.2f;

    [Header("OverDrive")]
    [SerializeField] private int overdriveDodgeFactor = 2;
    [SerializeField] private float overdriveSpeedFactor = 1.2f;
    [SerializeField] private float overDriveFireFactor = 1.2f;

    private Quaternion previousRotation;

    private Rigidbody2D playerRigidbody;
    private float paddingX;
    private float paddingY;

    private bool isOverdriving = false;

    private Coroutine moveCoroutine;

    private WaitForSeconds waitForFireInterval;
    private WaitForSeconds waitForOverdriveFireInterval;
    private WaitForSeconds waitHealthRegenerateTime;
    private WaitForSeconds waitDecelerationTime;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;

        playerRigidbody.gravityScale = 0;

        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval / overDriveFireFactor);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitDecelerationTime = new WaitForSeconds(decelerationTime);
    }
    private void OnEnable()
    {
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
    }
    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
    }

    private void Start()
    {

        input.EnableGameplayInput();
    }

    private void Update()
    {
    }

    #region Move
    private void Move(Vector2 moveInput)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        //moveDirection = moveInput.normalized;
        moveCoroutine = StartCoroutine(MoveCoroutine(
            accelerationTime,
            moveInput * moveSpeed,
            Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));

        StartCoroutine(nameof(MoveRangeLimitCoroutine));
    }

    private void StopMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));

        StopCoroutine(nameof(MoveRangeLimitCoroutine));
    }

    /// <summary>
    /// 移动协程
    /// </summary>
    /// <param name="duration">协程持续时间</param>
    /// <param name="moveVelocity">移动方向</param>
    /// <param name="moveRotation">旋转角度</param>
    /// <returns></returns>
    IEnumerator MoveCoroutine(float duration, Vector2 moveVelocity, Quaternion moveRotation)
    {
        float t = 0f;

        //previousRotation = transform.rotation;

        while (t < duration)
        {
            t += Time.fixedDeltaTime / duration;
            playerRigidbody.velocity = Vector2.Lerp(playerRigidbody.velocity, moveVelocity, t / duration);
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t / duration);

            yield return null;
        }
    }

    IEnumerator MoveRangeLimitCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.PlayerMoveablePosition(transform.position, paddingX, paddingY);

            yield return null;
        }
    }
    #endregion

    #region Fire
    void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }

    void StopFire()
    {
        StopCoroutine(nameof(FireCoroutine));
    }
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            switch (weaponPower)
            {
                case 0:
                    ObjectPoolManager.Release(isOverdriving ? projectileOverdrive : projectiles[1], muzzles[1].position);
                    break;
                case 1:
                    ObjectPoolManager.Release(isOverdriving ? projectileOverdrive : projectiles[1], muzzles[0].position);
                    ObjectPoolManager.Release(isOverdriving ? projectileOverdrive : projectiles[1], muzzles[2].position);
                    break;
                case 2:
                    ObjectPoolManager.Release(isOverdriving ? projectileOverdrive : projectiles[1], muzzles[1].position);
                    ObjectPoolManager.Release(isOverdriving ? projectileOverdrive : projectiles[0], muzzles[0].position);
                    ObjectPoolManager.Release(isOverdriving ? projectileOverdrive : projectiles[2], muzzles[2].position);
                    break;
                default:
                    break;
            }

            //AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);

            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
        }
    }
    #endregion
}