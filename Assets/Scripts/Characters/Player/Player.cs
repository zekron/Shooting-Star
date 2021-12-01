using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(MoveController))]
public class Player : Character
{
    [Header("Player")]
    [SerializeField] private FloatEventChannelSO shieldInitEventSO;
    [SerializeField] private FloatEventChannelSO shieldUpdateEventSO;
    [SerializeField] private PlayerInputSO input;
    [SerializeField] private PlayerProfileSO playerProfile;

    [Header("Regeneration")]
    [SerializeField] private bool regenerateHealth = true;
    [SerializeField] private float healthRegenerateTime;
    [SerializeField, Range(0f, 1f)] private float healthRegeneratePercent;

    [Header("Fire")]
    [SerializeField] private GameObject projectileOverdrive;
    [SerializeField, Range(0, 2)] private int weaponPower = 0;
    protected float fireInterval = 0.2f;

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
    private int overdriveDodgeFactor = 2;
    private float overdriveSpeedFactor = 1.2f;
    private float overdriveFireFactor = 1.2f;
    private MissileSystem missile;
    private bool isOverdriving = false;

    private Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;
    private MoveController moveController;

    private readonly float slowMotionDuration = 1f;

    private float coroutineTimer;
    private Coroutine moveCoroutine;
    private Coroutine healthRegenerateCoroutine;

    private WaitForSeconds waitForOverdriveFireInterval;
    private WaitForSeconds waitHealthRegenerateTime;
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    protected override void OnEnable()
    {
        base.OnEnable();

        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverdrive += OverDrive;
        input.onLaunchMissile += LaunchMissile;

        PlayerOverdrive.On += OpenOverdrive;
        PlayerOverdrive.Off += StopOverdrive;

        SetProfile();
    }

    private void OnDisable()
    {
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
        input.onOverdrive -= OverDrive;
        input.onLaunchMissile -= LaunchMissile;

        PlayerOverdrive.On -= OpenOverdrive;
        PlayerOverdrive.Off -= StopOverdrive;
    }

    private void OnValidate()
    {
        SetProfile();
    }

    protected override void Awake()
    {
        base.Awake();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        moveController = GetComponent<MoveController>();
        missile = GetComponent<MissileSystem>();

        maxRoll = dodgeDuration * rollSpeed;
        playerRigidbody.gravityScale = 0;

        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval / overdriveFireFactor);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
    }

    private void Start()
    {

        input.EnableGameplayInput();
        shieldInitEventSO.RaiseEvent(maxHealth);

#if UNITY_ANDROID
        StartCoroutine(nameof(FireCoroutine));
#endif
    }

    protected override void SetProfile()
    {
        moveController = GetComponent<MoveController>();

        maxHealth = playerProfile.MaxHealth;

        MoveSpeed = playerProfile.MoveSpeed;
        MoveRotationAngle = playerProfile.MoveRotationAngle;
        moveController.SetMoveProfile(MoveSpeed, MoveRotationAngle);

        fireInterval = playerProfile.FireInterval;

        overdriveDodgeFactor = playerProfile.OverdriveDodgeFactor;
        overdriveSpeedFactor = playerProfile.OverdriveSpeedFactor;
        overdriveFireFactor = playerProfile.OverdriveFireFactor;
    }

    #region HEALTH
    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        shieldUpdateEventSO.RaiseEvent(Health);
        TimeController.Instance.BulletTime(slowMotionDuration);

        if (gameObject.activeSelf)
        {
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
        shieldUpdateEventSO.RaiseEvent(Health);
    }

    public override void GetDie()
    {
        GameManager.onGameOver?.Invoke();
        GameManager.CurrentGameState = GameState.GameOver;
        shieldUpdateEventSO.RaiseEvent(Health = 0);
        base.GetDie();
    }
    #endregion

    public override void Move(Vector2 deltaMovement)
    {
        base.Move(deltaMovement);
        transform.position = Viewport.PlayerMoveablePosition(transform.position, paddingX, paddingY);
    }

    #region Fire
    protected override IEnumerator FireCoroutine()
    {
        while (true)
        {
            LoadProjectiles(weaponPower);
            switch (weaponPower)
            {
                case 0:
                    //ObjectPoolManager.Release(isOverdriving ? projectileOverdrive : projectiles[1], multiMuzzles[0].GetMuzzle(MuzzlePower.Double));
                    for (int i = 0; i < 3; i++)
                        ObjectPoolManager.Release(isOverdriving ? projectileOverdrive : projectiles[i], multiMuzzles[i].GetMuzzle(MuzzlePower.Double));
                    break;
                case 1:
                    //ObjectPoolManager.Release(isOverdriving ? projectileOverdrive : projectiles[1], multiMuzzles[0].GetMuzzle(MuzzlePower.Triple));
                    for (int i = 0; i < 5; i++)
                        ObjectPoolManager.Release(isOverdriving ? projectileOverdrive : projectiles[i], multiMuzzles[i].GetMuzzle(MuzzlePower.Triple));
                    break;
                case 2:
                    //ObjectPoolManager.Release(isOverdriving ? projectileOverdrive : projectiles[1], multiMuzzles[0].GetMuzzle(MuzzlePower.Quadruple));
                    for (int i = 0; i < projectiles.Length; i++)
                        ObjectPoolManager.Release(isOverdriving ? projectileOverdrive : projectiles[i], multiMuzzles[i].GetMuzzle(MuzzlePower.Quadruple));
                    break;
                default:
                    break;
            }

            AudioManager.Instance.PlaySFX(projectileLaunchSFX);
            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
        }
    }
    private void LoadProjectiles(int weaponPower)
    {

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
        PlayerEnergy.Instance.DrainEnergy(dodgeEnergyCost);
        playerCollider.isTrigger = true;
        currentRoll = 0f;
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);

        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.fixedDeltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.up);
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
        moveController.SetMoveSpeedByFactor(overdriveSpeedFactor);
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);
    }

    private void StopOverdrive()
    {
        isOverdriving = false;
        dodgeEnergyCost /= overdriveDodgeFactor;
        moveController.SetMoveSpeedByFactor(1 / overdriveSpeedFactor);
    }
    #endregion

    #region Launch Missile
    private void LaunchMissile()
    {
        missile.Launch(muzzles[1]);
    }
    #endregion
}