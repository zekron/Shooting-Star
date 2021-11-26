using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [SerializeField] private ShieldStatsBar statsBar_HUD;
    [SerializeField] private PlayerInputSO input;

    [Header("Regeneration")]
    [SerializeField] private bool regenerateHealth = true;
    [SerializeField] private float healthRegenerateTime;
    [SerializeField, Range(0f, 1f)] private float healthRegeneratePercent;

    [Header("Move")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float accelerationTime = 3f;
    [SerializeField] private float decelerationTime = 3f;
    [SerializeField] private float moveRotationAngle = 50f;
    private Vector2 moveDirection;
    private Vector2 tempPlayerVelocity;
    private Quaternion tempPlayerRotation;

    [Header("Fire")]
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private GameObject projectileOverdrive;
    [SerializeField] private Transform[] muzzles;
    [SerializeField] private AudioDataSO projectileLaunchSFX;
    [SerializeField, Range(0, 2)] private int weaponPower = 0;
    [SerializeField] private float fireInterval = 0.2f;

    [Header("Dodge")]
    [SerializeField] private AudioDataSO dodgeSFX;
    [SerializeField, Range(0, 100)] private int dodgeEnergyCost = 25;
    [SerializeField] private float dodgeDuration = 2;
    [SerializeField] private float rollSpeed = 360f;
    [SerializeField] private Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);
    private bool isDodging = false;
    private float currentRoll;
    private float maxRoll;

    [Header("OverDrive")]
    [SerializeField] private int overdriveDodgeFactor = 2;
    [SerializeField] private float overdriveSpeedFactor = 1.2f;
    [SerializeField] private float overDriveFireFactor = 1.2f;
    MissileSystem missile;
    private bool isOverdriving = false;

    private Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;

    private readonly float slowMotionDuration = 1f;

    private float coroutineTimer;
    private Coroutine moveCoroutine;
    private Coroutine healthRegenerateCoroutine;

    private WaitForSeconds waitForFireInterval;
    private WaitForSeconds waitForOverdriveFireInterval;
    private WaitForSeconds waitHealthRegenerateTime;
    private WaitForSeconds waitDecelerationTime;
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        missile = GetComponent<MissileSystem>();

        maxRoll = dodgeDuration * rollSpeed;
        playerRigidbody.gravityScale = 0;

        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval / overDriveFireFactor);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitDecelerationTime = new WaitForSeconds(decelerationTime);
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverdrive += OverDrive;
        input.onLaunchMissile += LaunchMissile;

        PlayerOverdrive.On += OpenOverdrive;
        PlayerOverdrive.Off += StopOverdrive;
    }

    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
        input.onOverdrive -= OverDrive;
        input.onLaunchMissile -= LaunchMissile;

        PlayerOverdrive.On -= OpenOverdrive;
        PlayerOverdrive.Off -= StopOverdrive;
    }

    private void Start()
    {

        input.EnableGameplayInput();
        statsBar_HUD.Initialize(health, maxHealth);

#if UNITY_ANDROID
        StartCoroutine(nameof(FireCoroutine));
#endif
    }

    #region HEALTH
    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        statsBar_HUD.UpdateStats(health, maxHealth);
        TimeController.Instance.BulletTime(slowMotionDuration);

        if (gameObject.activeSelf)
        {
            Move(moveDirection);

            if (regenerateHealth)
            {
                if (healthRegenerateCoroutine != null)
                {
                    StopCoroutine(healthRegenerateCoroutine);
                }

                healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
            }
        }
    }

    public override void GetHealing(float value)
    {
        base.GetHealing(value);
        statsBar_HUD.UpdateStats(health, maxHealth);
    }

    public override void GetDie()
    {
        GameManager.onGameOver?.Invoke();
        GameManager.CurrentGameState = GameState.GameOver;
        statsBar_HUD.UpdateStats(0f, maxHealth);
        base.GetDie();
    }
    #endregion

    #region Move
    private void Move(Vector2 moveInput)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveDirection = moveInput;
        moveCoroutine = StartCoroutine(MoveCoroutine(
            accelerationTime,
            moveInput * moveSpeed,
            Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));

        StopCoroutine(nameof(DecelerationCoroutine));
        StartCoroutine(nameof(MoveRangeLimatationCoroutine));
    }

    private void StopMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveDirection = Vector2.zero;
        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));

        StartCoroutine(nameof(DecelerationCoroutine));
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
        coroutineTimer = 0f;
        tempPlayerVelocity = playerRigidbody.velocity;
        tempPlayerRotation = transform.rotation;

        while (coroutineTimer < duration)
        {
            coroutineTimer += Time.fixedDeltaTime;
            playerRigidbody.velocity = Vector2.Lerp(tempPlayerVelocity, moveVelocity, coroutineTimer / duration);
            transform.rotation = Quaternion.Lerp(tempPlayerRotation, moveRotation, coroutineTimer / duration);

            yield return waitForFixedUpdate;
        }
    }

    IEnumerator MoveRangeLimatationCoroutine()
    {
        while (true)
        {
            //transform.position = Viewport.PlayerMoveablePosition(transform.position, paddingX, paddingY);

            yield return null;
        }
    }

    IEnumerator DecelerationCoroutine()
    {
        yield return waitDecelerationTime;

        StopCoroutine(nameof(MoveRangeLimatationCoroutine));
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

            AudioManager.Instance.PlaySFX(projectileLaunchSFX);

            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
        }
    }
    #endregion

    #region Dodge
    private void Dodge()
    {
        if (isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;

        StartCoroutine(nameof(DodgeCoroutine));
    }
    private IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        AudioManager.Instance.PlaySFX(dodgeSFX);
        PlayerEnergy.Instance.Cost(dodgeEnergyCost);
        playerCollider.isTrigger = true;
        currentRoll = 0f;
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);

        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.fixedDeltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);
            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);

            yield return waitForFixedUpdate;
        }

        playerCollider.isTrigger = false;
        isDodging = false;
    }
    #endregion

    #region OverDrive
    private void OverDrive()
    {
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;

        PlayerOverdrive.On.Invoke();
    }

    private void OpenOverdrive()
    {
        isOverdriving = true;
        dodgeEnergyCost *= overdriveDodgeFactor;
        moveSpeed *= overdriveSpeedFactor;
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);
    }

    private void StopOverdrive()
    {
        isOverdriving = false;
        dodgeEnergyCost /= overdriveDodgeFactor;
        moveSpeed /= overdriveSpeedFactor;
    }
    #endregion

    #region Launch Missile
    private void LaunchMissile()
    {
        missile.Launch(muzzles[1]);
    }
    #endregion
}